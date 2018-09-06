using Consumer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumerTest
{
    [TestFixture]
    public class ConsumerTest
    {
        [Test]
        public void ConsumersTest()
        {
            MainWindow window = null;

            // The dispatcher thread
            var t = new Thread(() =>
            {
                window = new MainWindow();

            // Initiates the dispatcher thread shutdown when the window closes
            window.Closed += (s, e) => window.Dispatcher.InvokeShutdown();

                window.Show();

            // Makes the thread support message pumping
            System.Windows.Threading.Dispatcher.Run();
            });

            // Configure the thread
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}
