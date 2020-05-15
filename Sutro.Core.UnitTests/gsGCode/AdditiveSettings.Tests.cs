using gs.info;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sutro.Core.UnitTests
{
    [TestClass]
    public class AdditiveSettingsTests
    {
        [TestMethod]
        public void Clone()
        {
            // Arrange
            var settings = new PrusaSettings();

            // Act
            var clone = settings.Clone();

            // Asert
            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(PrusaSettings));
        }

    }
}
