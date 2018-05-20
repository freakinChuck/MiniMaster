using MiniMaster.Storage;
using MiniMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OfficeOpenXml;
using MiniMaster.Acolyte;

namespace MiniMaster.RessourceScheduling
{
    public sealed class RessourceScheduleManager : IDisposable
    {
        public RessourceScheduleManager() { }

        public void GenerateScheduleForPeriod(DateTime startDate, DateTime endDate)
        {
            if (Workspace.HasCurrentWorkspaceChanges)
            {
                var result = MessageBox.Show("Sie haben noch ungespeicherte Änderungen. Möchten Sie diese speichern, bevor Sie fortfahren?", "Ungespeicherte Änderungen", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Workspace.SaveCurrentWorkspace();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            var services = this.GetServicesInPeriod(startDate, endDate);
            foreach (var service in services)
            {
                try
                {
                    this.SetAcolytesForService(service);
                }
                catch (IndexOutOfRangeException)
                {
                    break;
                }
            }
        }

        public void ExportScheduleForPeriod(ExcelWorksheet worksheet, DateTime startDate, DateTime endDate)
        {
            if (Workspace.HasCurrentWorkspaceChanges)
            {
                var result = MessageBox.Show("Sie haben noch ungespeicherte Änderungen. Möchten Sie diese speichern, bevor Sie fortfahren?", "Ungespeicherte Änderungen", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Workspace.SaveCurrentWorkspace();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            int rowCounter = 1;

            var services = this.GetServicesInPeriod(startDate, endDate);
            foreach (var service in services)
            {
                worksheet.SetValue(rowCounter, 1,
                    $"{ContinousAbsenceViewModel.GetTagname(service.DateAndTime.DayOfWeek)} {service.DateAndTime.ToString("dd.MM.yyyy")}");

                worksheet.SetValue(rowCounter, 2,
                    $"{service.DateAndTime.ToString("HH:mm")}");

                var jobsOfService = this.GetJobsForService(service.Id).GroupBy(x => x.JobId).Select(x => new
                {
                    Job = Workspace.CurrentData.Jobs.FirstOrDefault(j => j.Id == x.Key),
                    Acolytes = x.Select(j => j.AcolyteId).ToList()
                });

                foreach (var job in jobsOfService.OrderBy(x => x?.Job.Order))
                {
                    worksheet.SetValue(rowCounter, 3,
                        $"{job.Job?.Text ?? "unbekannte Aufgabe"}"
                    );
                    int columnCounter = 4;
                    foreach (var acolyteId in job.Acolytes)
                    {
                        worksheet.SetValue(rowCounter, columnCounter++, GetAssignedAcolyteName(acolyteId));

                                            
                    }                   

                    rowCounter++;
                }

                rowCounter++;
            }
        }

        private string GetAssignedAcolyteName(string assignedAcolyteId)
        {
            if (string.IsNullOrEmpty(assignedAcolyteId))
            {
                return "keine Zuteilung";
            }
            else
            {
                var acolyte = Workspace.CurrentData.Acolytes.FirstOrDefault(x => x.Id == assignedAcolyteId);
                var acolyteName = acolyte != null ? acolyte.Firstname + " " + acolyte.Name : "unbekannter Ministrant";
                return acolyteName;
            }
        }

        private void SetAcolytesForService(ServiceModel service)
        {
            bool onlyOlder = service.OnlyOlderAcolytes;
            bool twoOlder = service.TwoOlderAcolytes;

            var jobs = this.GetJobsForService(service.Id);
            var allPossibleAcolytes = this.GetPossibleAcolytesForService(service.DateAndTime);

            List<AcolyteModel> acolytesForJob = new List<AcolyteModel>();
            var count = 0;
            var countOfNeededAcolytes = jobs.Count;
            int requiredOlderAcolytes = GetRequiredNumberOfOlder(onlyOlder, twoOlder, countOfNeededAcolytes);

            for (int acolyteCount = 0; acolyteCount < allPossibleAcolytes.Count; acolyteCount++)
            {
                var tmpAcolyte = allPossibleAcolytes[count++];

                if (acolytesForJob.Contains(tmpAcolyte))
                {
                    continue;
                }

                var family = allPossibleAcolytes.Where(x => x.Id != tmpAcolyte.Id && !string.IsNullOrWhiteSpace(x.FamilyKey) && x.FamilyKey == tmpAcolyte.FamilyKey).ToList();
                family.Insert(0, tmpAcolyte);

                if (family.Count > countOfNeededAcolytes)
                {
                    continue;
                }

                if (requiredOlderAcolytes > 0)
                {
                    int numberOfAcolytesToCountAsOlder, numberOfAcolytesToCountAsMinor;
                    CalculateNumberOfOlderAcolytesByFamily(family, out numberOfAcolytesToCountAsOlder, out numberOfAcolytesToCountAsMinor);
                    if (numberOfAcolytesToCountAsMinor > (countOfNeededAcolytes - requiredOlderAcolytes))
                    {
                        continue;
                    }

                    requiredOlderAcolytes -= numberOfAcolytesToCountAsOlder;
                }
                countOfNeededAcolytes -= family.Count;
                SetFamilyForJob(acolytesForJob, family);
                if (acolytesForJob.Count >= jobs.Count)
                {
                    break;
                }
            }

            if (acolytesForJob.Count < jobs.Count)
            {
                MessageBox.Show("Der Gottesdienst vom " + service.DateAndTime.ToString() + " benötigt mehr Ministranten als für dieses Datum verfügbar sind." + Environment.NewLine +
                    "Die Planungserstellung wurde angehalten.");
                throw new IndexOutOfRangeException();
            }

            SetAcolytesForJob(jobs, acolytesForJob);
        }

        private void CalculateNumberOfOlderAcolytesByFamily(List<AcolyteModel> family, out int numberOfAcolytesToCountAsOlder, out int numberOfAcolytesToCountAsMinor)
        {
            var numberOfRealOlderAcolytes = family.Count(x => IsAcolyteAnOlderAcolyte(x, false));
            numberOfAcolytesToCountAsOlder = 0;
            if (numberOfRealOlderAcolytes > 0)
            {
                numberOfAcolytesToCountAsOlder = family.Count(x => IsAcolyteAnOlderAcolyte(x, true));
            }
            numberOfAcolytesToCountAsMinor = family.Count - numberOfAcolytesToCountAsOlder;
        }

        private static void SetAcolytesForJob(List<ServiceJobModel> jobs, List<AcolyteModel> acolytesForJob)
        {
            foreach (var job in jobs)
            {
                if (string.IsNullOrEmpty(job.AcolyteId))
                {
                    var acolyte = acolytesForJob.FirstOrDefault();
                    if (acolyte != null)
                    {
                        job.AcolyteId = acolyte.Id;
                        acolytesForJob.Remove(acolyte);
                        Workspace.RegisterDataChanged();
                    }
                }
            }
        }

        private static void SetFamilyForJob(List<AcolyteModel> acolytesForJob, List<AcolyteModel> family)
        {
            foreach (var acolyte in family)
            {
                acolytesForJob.Add(acolyte);
            }
        }

        private static int GetRequiredNumberOfOlder(bool onlyOlder, bool twoOlder, int countOfNeededAcolytes)
        {
            int numberRequired;
            if (onlyOlder)
            {
                numberRequired = countOfNeededAcolytes;
            }
            else if (twoOlder)
            {
                numberRequired = 2;
            }
            else
            {
                numberRequired = 0;
            }
            return numberRequired;
        }

        private bool IsAcolyteAnOlderAcolyte(AcolyteModel acolyte, bool hasAlreadyOlderSiblings)
        {
            var timeSinceEntry = DateTime.Today - (acolyte.Entry ?? DateTime.MinValue);
            return timeSinceEntry.TotalDays > 365 * (hasAlreadyOlderSiblings ? 2 : 4);
        }

        public List<AcolyteModel> GetPossibleAcolytesForService(DateTime serviceDateTime)
        {
            List<AcolyteModel> possibleAcolytes = new List<AcolyteModel>();
            var allAcolytes = Workspace.CurrentData.Acolytes;

            List<string> familyWithAbsences = new List<string>();
            foreach (var acolyte in allAcolytes)
            {
                bool hasAbsence = Workspace.CurrentData.Absences.Any(x =>
                                    x.AcolyteId == acolyte.Id &&
                                    (x.DateAndTime == serviceDateTime || (x.DateAndTime.Date == serviceDateTime.Date && x.WholeDay)));
                bool hasContinousAbsence = Workspace.CurrentData.ContinousAbsences.Any(x => x.AcolyteId == acolyte.Id && TimeSpan.Parse(string.IsNullOrEmpty(x.Time) ? "00:00" : x.Time) == serviceDateTime.TimeOfDay && x.Day == serviceDateTime.DayOfWeek);
                if (!hasAbsence && !hasContinousAbsence)
                {
                    possibleAcolytes.Add(acolyte);
                }
                else
                {
                    familyWithAbsences.Add(acolyte.FamilyKey);
                }
            }

            possibleAcolytes = possibleAcolytes.Where(x => string.IsNullOrEmpty(x.FamilyKey) || !familyWithAbsences.Contains(x.FamilyKey)).ToList();

            return OrderAcolyteList(possibleAcolytes);
        }

        private List<AcolyteModel> OrderAcolyteList(List<AcolyteModel> possibleAcolytes)
        {
            var maxEntry = Workspace.CurrentData.Acolytes.Max(x => x.Entry) ?? DateTime.MinValue;
            var allServicesWithJobs = Workspace.CurrentData.Services.Where(x => x.DateAndTime > maxEntry).Select(x => new
            {
                serviceId = x.Id,
                acolytes = Workspace.CurrentData.ServiceJobs.Where(j => j.ServiceId == x.Id).Select(j => j.AcolyteId).ToList()
            }).ToList();

            var acolyteCountMap = possibleAcolytes.ToDictionary(x => x.Id, x => allServicesWithJobs.Count(s => s.acolytes.Any(a => a == x.Id)));

            return possibleAcolytes.OrderBy(x => acolyteCountMap[x.Id]).ToList();
        }

        public bool ClearScheduleForPeriod(DateTime startDate, DateTime endDate)
        {
            if (Workspace.HasCurrentWorkspaceChanges)
            {
                var result = MessageBox.Show("Sie haben noch ungespeicherte Änderungen. Möchten Sie diese speichern, bevor Sie fortfahren?", "Ungespeicherte Änderungen", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Workspace.SaveCurrentWorkspace();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return true;
                }
            }

            bool hasDoneChanges = false;
            var services = this.GetServicesInPeriod(startDate, endDate);
            foreach (var service in services)
            {
                var jobs = this.GetJobsForService(service.Id);
                foreach (var job in jobs)
                {
                    if (!string.IsNullOrEmpty(job.AcolyteId))
                    {
                        hasDoneChanges = true;
                        job.AcolyteId = null;
                        Workspace.RegisterDataChanged();
                    }
                }
            }
            return hasDoneChanges;
        }

        private List<ServiceModel> GetServicesInPeriod(DateTime startDate, DateTime endDate)
        {
            return Workspace.CurrentData.Services.Where(s => s.DateAndTime.Date >= startDate.Date && s.DateAndTime.Date <= endDate.Date).OrderBy(x => x.DateAndTime).ToList();
        }
        private List<ServiceJobModel> GetJobsForService(string serviceId)
        {
            return Workspace.CurrentData.ServiceJobs.Where(j => j.ServiceId == serviceId).ToList();
        }

        public void Dispose()
        {
            // do Nothing
        }
    }
}
