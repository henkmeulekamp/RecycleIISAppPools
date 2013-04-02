using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace RecycleIISAppPools.UnitTests
{
    [TestFixture]
    public class NameFilterTests
    {
        readonly List<string> _names = new List<string>
                {
                    "Product", "product", 
                    "aproduct", "someProduct", 
                    "someProductEnds", "someProductOther", 
                    "ProductOther", "productother"
                };

        [Test]
        [TestCase("product1", null)]
        [TestCase("product", "Product,product")]
        [TestCase("productother", "ProductOther,productother")]
        [TestCase("someProductEnds", "someProductEnds")]
        public void Filter(string filterlist, string contains)
        {
            FilterTest(filterlist, contains);
        }

        [Test]
        [TestCase("testing*", null)]
        [TestCase("product*", "Product,product,productother,")]
        [TestCase("productoth*", "ProductOther,productother")]
        [TestCase("someProductEnds*", "someProductEnds,someProductEnds")]
        public void FilterWithWildCardAtEnd(string filterlist, string contains)
        {
            FilterTest(filterlist, contains);
        }

        [Test]
        [TestCase("*testing", null)]
        [TestCase("*product", "product,Product,aproduct,someProduct")]
        [TestCase("*someProductEnds", "someProductEnds,someProductEnds")]
        public void FilterWithWildCardAtstart(string filterlist, string contains)
        {
            FilterTest(filterlist, contains);
        }

        [Test]
        [TestCase("*testing*", null)]
        [TestCase("*product*", "product,Product,aproduct,someProduct,someProductEnds,someProductOther,ProductOther,productother")]
        [TestCase("*someProductE*", "someProductEnds,someProductEnds")]
        public void FilterWithWildCards(string filterlist, string contains)
        {
            FilterTest(filterlist, contains);
        }

        private void FilterTest(string filterlist, string contains)
        {
            var filters = new List<string>(filterlist.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries));
            var filtered = NameFilter.Filter(filters, _names);

            if (contains != null)
            {
                var enumerable = filtered as string[] ?? filtered.ToArray();

                foreach (var c in contains.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries))
                {
                    Assert.Contains(c, enumerable.ToArray());
                }
            }
            else
            {
                Assert.IsEmpty(filtered);
            }
        }
    }
}
