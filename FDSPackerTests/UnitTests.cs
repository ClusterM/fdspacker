using wtf.cluster.FDSPacker;

namespace wtf.cluster.FDSPackerTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            TestRom(Path.Combine("TestFiles", "test1.fds"));
        }

        [TestMethod]
        public void TestMethod2()
        {
            TestRom(Path.Combine("TestFiles", "test2.fds"));
        }

        [TestMethod]
        public void TestMethod3()
        {
            TestRom(Path.Combine("TestFiles", "test3.fds"));
        }

        [TestMethod]
        public void TestMethod4()
        {
            TestRom(Path.Combine("TestFiles", "test4.fds"));
        }

        [TestMethod]
        public void TestMethod5()
        {
            TestRom(Path.Combine("TestFiles", "test5.fds"));
        }

        [TestMethod]
        public void TestMethod6()
        {
            TestRom(Path.Combine("TestFiles", "test6.fds"));
        }

        [TestMethod]
        public void TestMethod7()
        {
            TestRom(Path.Combine("TestFiles", "test7.fds"));
        }

        static void TestRom(string filename)
        {
            // Unpack
            var tempDir = Path.Combine(Path.GetTempPath(), filename);
            Directory.CreateDirectory(tempDir);
            FdsPackUnpack.Unpack(new UnpackOptions(noUnknown: false, quiet: false, inputFile: filename, outputDir: tempDir));

            // Pack
            var tempRom = Path.GetTempFileName();
            FdsPackUnpack.Pack(new PackOptions(false, false, tempDir, tempRom));
            Directory.Delete(tempDir, true);

            // Compare
            var fin = File.ReadAllBytes(filename);
            var fout = File.ReadAllBytes(tempRom);
            Assert.AreEqual(fin.Length, fout.Length);
            for (var i = 0; i < fin.Length; i++)
                Assert.AreEqual(fin[i], fout[i], $"Byte 0x{i:X06}");
            File.Delete(tempRom);
        }
    }
}