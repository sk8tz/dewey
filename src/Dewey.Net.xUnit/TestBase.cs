using System;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Dewey.Net.xUnit
{
    public abstract class TestBase : IDisposable
    {
        private readonly ITestOutputHelper _output;

        protected TestBase(ITestOutputHelper output)
        {
            _output = output;

            Setup();
        }

        public virtual void Setup()
        {
            // Reserved for Test Setup
        }

        public void Print(string message)
        {
            _output.WriteLine(message);
        }

        public void Print(object value)
        {
            var result = JsonConvert.SerializeObject(value);

            _output.WriteLine(result);
        }

        public void Print(string message, params object[] args)
        {
            _output.WriteLine(message, args);
        }

        public virtual void Teardown()
        {
            // Reserved for Test Teardown
        }

        #region IDisposable

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                    Teardown();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}