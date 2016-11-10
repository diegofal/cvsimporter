using CsvImporter.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvImporter.Extensions
{
    public static class ContactListExtensions
    {
        public static IEnumerable<ContactFrequency> GetWordFrequency(this List<Contact> list, Func<dynamic, string> keySelector)
        {
            return list.GroupBy(keySelector)
                     .Select(group => new ContactFrequency{ Name = group.Key, Frequency = group.Count() })
                     .OrderByDescending(result => result.Frequency).ThenBy(result => result.Name);
        }
    }
}
