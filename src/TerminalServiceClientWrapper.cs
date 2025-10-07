using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace TerminalInquiryLib
{
    /// <summary>
    /// Wrapper for the TRNService WCF client.
    /// IMPORTANT: This project expects you to generate a proxy class from the WSDL
    /// and place it under the namespace TerminalInquiryLib.ConnectedServices.TRNService
    /// (see GenerateProxy.ps1).
    /// </summary>
    public class TerminalServiceClientWrapper : IDisposable
    {
        private object _generatedClient;
        private readonly string _endpointAddress;
        private readonly Binding _binding;
        private bool _disposed;

        /// <summary>
        /// Create wrapper with an optional custom endpoint address.
        /// If you generated the proxy with the same endpoint/address as the WSDL, you can omit the address.
        /// </summary>
        /// <param name="endpointAddress">Optional: full URL of the service endpoint</param>
        public TerminalServiceClientWrapper(string? endpointAddress = null)
        {
            _endpointAddress = endpointAddress ?? string.Empty;

            // Default to BasicHttpBinding. Adjust if your service requires wsHttpBinding or custom security.
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = 20000000;
            binding.ReaderQuotas.MaxArrayLength = 20000000;
            binding.SendTimeout = TimeSpan.FromSeconds(30);
            binding.OpenTimeout = TimeSpan.FromSeconds(15);
            _binding = binding;

            // Try to instantiate generated proxy if present.
            TryCreateGeneratedClient();
        }

        private void TryCreateGeneratedClient()
        {
            // The generated proxy (if produced by dotnet-svcutil) is typically a ClientBase<T> named e.g. TRNServiceClient
            // under namespace TerminalInquiryLib.ConnectedServices.TRNService.
            // We attempt to locate and instantiate it via reflection to avoid hard compile-time coupling
            // before the user generates the proxy.
            try
            {
                var asm = typeof(TerminalServiceClientWrapper).Assembly;
                var ns = "TerminalInquiryLib.ConnectedServices.TRNService";
                // common generated client class name
                var clientType = asm.GetType(ns + ".TRNServiceClient") ?? asm.GetType(ns + ".TRNServiceClientChannel") ?? asm.GetType(ns + ".TRNServiceSoapClient");
                if (clientType == null)
                    return;

                if (!string.IsNullOrEmpty(_endpointAddress))
                {
                    // Try constructor (Binding endpoint) => new TRNServiceClient(Binding, EndpointAddress)
                    var ctor = clientType.GetConstructor(new Type[] { typeof(Binding), typeof(EndpointAddress) });
                    if (ctor != null)
                    {
                        _generatedClient = ctor.Invoke(new object[] { _binding, new EndpointAddress(_endpointAddress) });
                        return;
                    }

                    // Try constructor (string endpointConfigurationName)
                    ctor = clientType.GetConstructor(new Type[] { typeof(string) });
                    if (ctor != null)
                    {
                        _generatedClient = ctor.Invoke(new object[] { _endpointAddress });
                        return;
                    }
                }
                else
                {
                    // Try parameterless constructor
                    var ctor = clientType.GetConstructor(Type.EmptyTypes);
                    if (ctor != null)
                    {
                        _generatedClient = ctor.Invoke(new object[] { });
                        return;
                    }
                }
            }
            catch
            {
                // swallow; we'll throw helpful messages when methods are invoked.
            }
        }

        /// <summary>
        /// Example wrapper method. Replace/extend with the actual methods generated from WSDL.
        /// This method attempts to call a generated method "GetDriverByNational_ID" via reflection.
        /// If no generated proxy exists, an exception with instructions is thrown.
        /// </summary>
        public object GetDriverByNational_ID(string nationalId)
        {
            if (_generatedClient == null)
                throw new InvalidOperationException("Generated proxy class not found. Run GenerateProxy.ps1 to create TRNService proxy from the WSDL and rebuild the library. See README.md for details.");

            var method = _generatedClient.GetType().GetMethod("GetDriverByNational_ID");
            if (method == null)
                throw new MissingMethodException("Generated proxy does not contain 'GetDriverByNational_ID'. Check the generated proxy or call the correct method name as exposed by the WSDL.");

            try
            {
                var result = method.Invoke(_generatedClient, new object[] { nationalId });
                return result!;
            }
            catch (System.Reflection.TargetInvocationException tie) when (tie.InnerException != null)
            {
                throw tie.InnerException;
            }
        }

        /// <summary>
        /// Dispose and close generated client gracefully.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            if (_generatedClient == null) return;

            try
            {
                var closeMethod = _generatedClient.GetType().GetMethod("Close");
                var abortMethod = _generatedClient.GetType().GetMethod("Abort");
                if (closeMethod != null)
                    closeMethod.Invoke(_generatedClient, null);
            }
            catch
            {
                try
                {
                    var abortMethod = _generatedClient?.GetType().GetMethod("Abort");
                    abortMethod?.Invoke(_generatedClient, null);
                }
                catch { }
            }
        }
    }
}
