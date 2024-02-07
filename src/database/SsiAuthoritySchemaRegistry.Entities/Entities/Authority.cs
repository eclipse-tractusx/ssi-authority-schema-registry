namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities.Entities;

public class Authority
{
    public Authority(string bpn)
    {
        Bpn = bpn;
    }

    public string Bpn { get; set; }

    public ICollection<CredentialAuthority> CredentialAuthorities { get; private set; } = new HashSet<CredentialAuthority>();
}
