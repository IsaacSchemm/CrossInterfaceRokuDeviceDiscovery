# CrossInterfaceRokuDeviceDiscovery

An extension of [RokuDotNet](https://github.com/philliphoff/RokuDotNet) that
allows UDP device discovery to work on specific network interfaces (based on
their IP addresses) or across multiple interfaces.

Example usage:

    // C#

    // Broadcast on all IPv4 addresses
    var addresses = Dns.GetHostEntry(Dns.GetHostName())
        .AddressList
        .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
        .ToList();
    IRokuDeviceDiscoveryClient client = new CrossInterfaceRokuDeviceDiscoveryClient(addresses);
    await client.DiscoverDevicesAsync(async context => {
        Console.WriteLine(context.Device.Id);
        return false;
    });
