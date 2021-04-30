using RokuDotNet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CrossInterfaceRokuDeviceDiscovery {
	public sealed class CrossInterfaceRokuDeviceDiscoveryClient : IRokuDeviceDiscoveryClient {
		private readonly IReadOnlyList<ExplicitInterfaceRokuDeviceDiscoveryClient> _clients;

		public CrossInterfaceRokuDeviceDiscoveryClient(IEnumerable<IPEndPoint> endPoints, HttpMessageHandler handler = null) {
			_clients = endPoints.Select(x => new ExplicitInterfaceRokuDeviceDiscoveryClient(x, handler)).ToList();
			foreach (var c in _clients)
				c.DeviceDiscovered += (sender, e) => this.DeviceDiscovered?.Invoke(sender, e);
		}

		public CrossInterfaceRokuDeviceDiscoveryClient(IEnumerable<IPAddress> addresses, HttpMessageHandler handler = null)
			: this(addresses.Select(x => new IPEndPoint(x, 0)), handler) { }

		public event EventHandler<DeviceDiscoveredEventArgs> DeviceDiscovered;

		public Task DiscoverDevicesAsync(CancellationToken cancellationToken = default) {
			return Task.WhenAll(_clients.Select(c => c.DiscoverDevicesAsync(cancellationToken)));
		}

		public Task DiscoverDevicesAsync(Func<DiscoveredDeviceContext, Task<bool>> onDeviceDiscovered, CancellationToken cancellationToken = default) {
			return Task.WhenAll(_clients.Select(c => c.DiscoverDevicesAsync(onDeviceDiscovered, cancellationToken)));
		}
	}
}