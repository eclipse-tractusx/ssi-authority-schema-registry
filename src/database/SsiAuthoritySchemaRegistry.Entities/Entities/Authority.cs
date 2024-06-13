namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities.Entities;

public class Authority(string bpn)
{
    public string Bpn { get; set; } = bpn;

    public ICollection<CredentialAuthority> CredentialAuthorities { get; private set; } = new HashSet<CredentialAuthority>();
}
