using System.Security.Cryptography.X509Certificates;

var store = new X509Store("MY", StoreLocation.CurrentUser);
store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
var collection = store.Certificates;
var filteredCerts = new X509Certificate2Collection();
foreach(var certificate in collection.Where(x => x.NotAfter > DateTime.UtcNow && x.HasPrivateKey).OrderBy(x => x.GetNameInfo(X509NameType.SimpleName, true)))
{
    filteredCerts.Add(certificate);
}

var cert = X509Certificate2UI.SelectFromCollection(filteredCerts, "Select", "Select a certificate to sign", X509SelectionFlag.SingleSelection);
if (cert == null)
{
    return;
}
var certBase64 = Convert.ToBase64String(cert.Export(X509ContentType.Cert));

Console.WriteLine($"Name - {cert[0].Subject}");
Console.WriteLine($"Issuer - {cert[0].IssuerName.Name}");
Console.WriteLine($"Thumbprint - {cert[0].Thumbprint}");
Console.WriteLine($"S/N - {cert[0].SerialNumber}");
Console.WriteLine($"Base64 - {certBase64}");
Console.WriteLine("-----------------------------------------");
Console.WriteLine();

Console.WriteLine("Select which attr copy to clipboard");
Console.WriteLine("1. Thumbprint");
Console.WriteLine("2. S/N");
Console.WriteLine("3. Base 64 (can paste raw to request header)");
var volba = Console.ReadKey();
switch (volba.KeyChar)
{
    case '1':
        TextCopy.ClipboardService.SetText(cert[0].Thumbprint);
        break;
    case '2':
        TextCopy.ClipboardService.SetText(cert[0].SerialNumber);
        break;
    case '3':
        TextCopy.ClipboardService.SetText(certBase64);
        break;
}
Console.WriteLine();
Console.WriteLine("⚠️ Value was copied to clipboard. ⚠️");
Console.WriteLine();
Console.WriteLine("Press any key to continue.");
Console.ReadLine();