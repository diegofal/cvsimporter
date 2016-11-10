using CsvHelper;
using CsvImporter.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvImporter.Extensions;

namespace CsvImporter.Services
{
    public class CsvProcessor
    {
        private const string NAMES_FILE = "names_freq.txt";
        private const string ADDRESSES_FILE = "addresses.txt";

        public void ProcessCsv(string fileName)
        {
            try
            {
                using (var file = File.OpenText(fileName))
                {
                    var contacts = new CsvReader(file)
                                       .GetRecords<Contact>()
                                       .ToList();
                    var outputDir = Path.GetDirectoryName(fileName);

                    ProcessNames(contacts, outputDir);
                    ProcessAddresses(contacts, outputDir);
                }
            }
            catch (Exception exception)
            {
                // TODO: Log exception
                throw exception;
            }
        }

        private void ProcessNames(List<Contact> contacts, string outputDir)
        {
            var firstNames = contacts.GetWordFrequency(contact => contact.FirstName);
            var lastNames = contacts.GetWordFrequency(contact => contact.LastName);

            Action<IEnumerable<dynamic>, StreamWriter> PrintToFile = (list, file) =>
            {
                foreach (var element in list)
                {
                    file.WriteLine($"{element.Name}: {element.Frequency}");
                }
            };

            using (var outputFile = File.CreateText($"{outputDir}/{NAMES_FILE}"))
            {
                outputFile.WriteLine("First Name Frequencies:"  + Environment.NewLine);

                PrintToFile(firstNames, outputFile);

                outputFile.WriteLine("--------------------------");
                outputFile.WriteLine("Last Name Frequencies:" + Environment.NewLine);

                PrintToFile(lastNames, outputFile);
            }
        }

        private void ProcessAddresses(List<Contact> contacts, string outputDir)
        {
            var sortedByAddressContacts = contacts
                   .OrderBy(contact => contact.Address, new AddressComparer())
                   .ToList();

            using (var outputFile = File.CreateText($"{outputDir}/{ADDRESSES_FILE}"))
            {
                foreach (var contact in sortedByAddressContacts)
                {
                    outputFile.WriteLine(contact.Address);
                }
            }
        }
    }

    class AddressComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            var addressXParts = x.Split(' ');
            var addressNameX = String.Join(" ", addressXParts.Skip(1).Take(addressXParts.Length - 1));

            var addressYParts = y.Split(' ');
            var addressNameY = String.Join(" ", addressYParts.Skip(1).Take(addressYParts.Length - 1));

            return addressNameX.CompareTo(addressNameY);
        }
    }
}
