using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsvImporter.Domain;
using System.Collections.Generic;
using CsvImporter.Extensions;
using System.Linq;

namespace CsvImporter.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void GetWordFrequencyTest()
        {
            List<Contact> contacts = new List<Contact>
            {
                new Contact { FirstName = "C", LastName = "A" },
                new Contact { FirstName = "B", LastName = "A" },
                new Contact { FirstName = "B", LastName = "A" },
                new Contact { FirstName = "A", LastName = "A" },
                new Contact { FirstName = "A", LastName = "A" },
                new Contact { FirstName = "C", LastName = "A" },
                new Contact { FirstName = "C", LastName = "A" },
                new Contact { FirstName = "C", LastName = "A" },
            };

            // Test first name frequencies
            var freqFirstName = contacts.GetWordFrequency(contact => contact.FirstName).ToList();

            Assert.AreEqual(freqFirstName.Count(), 3);

            Assert.AreEqual(freqFirstName[0].Frequency, 4);
            Assert.AreEqual(freqFirstName[0].Name, "C");

            // Test that same frequency is order correctly
            Assert.AreEqual(freqFirstName[1].Frequency, 2);
            Assert.AreEqual(freqFirstName[1].Name, "A");

            Assert.AreEqual(freqFirstName[2].Frequency, 2);
            Assert.AreEqual(freqFirstName[2].Name, "B");

            // Test last name frequencies
            var freqLastName = contacts.GetWordFrequency(contact => contact.LastName).ToList();

            Assert.AreEqual(freqLastName.Count(), 1);

            Assert.AreEqual(freqLastName[0].Frequency, 8);
            Assert.AreEqual(freqLastName[0].Name, "A");
        }
    }
}
