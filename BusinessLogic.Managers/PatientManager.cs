using Services.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Managers
{
    public class PatientManager
    {
        private readonly string _filePath;
        private readonly List<string> _bloodGroups = new() { "A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-" };

        public PatientManager(IConfiguration config)
        {
            _filePath = config["PatientFile"];
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
        }
        private List<Patient> LoadAll()
        {
            if (!File.Exists(_filePath)) return new();
            return File.ReadAllLines(_filePath)
                       .Where(l => !string.IsNullOrWhiteSpace(l))
                       .Select(l => l.Split(','))
                       .Select(a => new Patient {
                           Name = a[0],
                           LastName = a[1],
                           CI = a[2],
                           BloodGroup = a[3]
                       }).ToList();
        }
        private void SaveAll(List<Patient> list)
        {
            var lines = list.Select(p => $"{p.Name},{p.LastName},{p.CI},{p.BloodGroup}");
            File.WriteAllLines(_filePath, lines);
        }
        public List<Patient> GetAllPatients() => LoadAll();
        public Patient GetPatient(string ci) => LoadAll().FirstOrDefault(p => p.CI == ci);
        public Patient CreatePatient(Patient newPatient)
        {
            var all = LoadAll();
            if (all.Any(p => p.CI == newPatient.CI))
                throw new InvalidOperationException("Patient already exists");
            newPatient.BloodGroup = _bloodGroups[new Random().Next(_bloodGroups.Count)];
            all.Add(newPatient);
            SaveAll(all);
            return newPatient;
        }
        public Patient UpdatePatient(string ci, Patient update)
        {
            var all = LoadAll();
            var existing = all.FirstOrDefault(p => p.CI == ci)
                ?? throw new KeyNotFoundException("Patient not found");
            existing.Name = update.Name;
            existing.LastName = update.LastName;
            SaveAll(all);
            return existing;
        }
        public void DeletePatient(string ci)
        {
            var all = LoadAll();
            var existing = all.FirstOrDefault(p => p.CI == ci)
                ?? throw new KeyNotFoundException("Patient not found");
            all.Remove(existing);
            SaveAll(all);
        }
    }
}
