using System.DirectoryServices.Protocols;
using System.Net;
using Microsoft.Extensions.Options;

namespace cucuota;

public class ConfigLDAP
{
    public string Server { get; set; }
    public int ServerPort { get; set; }
    public string UserDN { get; set; }
    public string PasswordDN { get; set; }
    public string DN { get; set; }
    
    public string serverPath =>  Server;
    public int serverPort =>  ServerPort;
    public string userDN => UserDN;
    public string userDNPassword => PasswordDN;
    public string distinguishedNames => DN;

}

public class UserLDAP
{
    public string Username { get; set; }
    public string Password { get; set; }
}
public class LDAPUtils
{
    private readonly ConfigLDAP _serverLDAP;
    public LDAPUtils(IOptions<ConfigLDAP> serverLDAP)
    {
        _serverLDAP = serverLDAP.Value;
    }
    public bool SearchUser(string usernameToSearch)
    {
        using (LdapConnection cn = new LdapConnection(new LdapDirectoryIdentifier(_serverLDAP.serverPath, _serverLDAP.serverPort)))
        {
            cn.SessionOptions.ProtocolVersion = 3;
            cn.AuthType = AuthType.Basic;
            try
            {
                cn.Bind(new NetworkCredential(_serverLDAP.userDN, _serverLDAP.userDNPassword));
                // Define la consulta LDAP
                string filter = $"(&(objectClass=user)(sAMAccountName={usernameToSearch}))";
                string[] attributesToRetrieve = new string[] { "*" }; // Ajusta los atributos que deseas recuperar

                SearchRequest searchRequest = new SearchRequest(
                    _serverLDAP.distinguishedNames,
                    filter,
                    SearchScope.Subtree,
                    attributesToRetrieve
                );

                SearchResponse searchResponse = (SearchResponse)cn.SendRequest(searchRequest);

                if (searchResponse.Entries.Count > 0)
                {
                    // Se encontró al usuario
                    foreach (SearchResultEntry entry in searchResponse.Entries)
                    {
                        string displayName = entry.Attributes["displayName"][0].ToString();
                        string email = entry.Attributes["mail"][0].ToString();
                        string whenchanged = entry.Attributes["whenchanged"][0].ToString();
                        string badpwdcount = entry.Attributes["badpwdcount"][0].ToString();
                        string badpasswordtime = entry.Attributes["badpasswordtime"][0].ToString();
                        string whencreated = entry.Attributes["whencreated"][0].ToString();
                        string lastlogoff = entry.Attributes["lastlogoff"][0].ToString();
                        string physicaldeliveryofficename = entry.Attributes["physicaldeliveryofficename"][0].ToString();
                        DateTime lastlogon = DateTime.FromFileTime(long.Parse(entry.Attributes["lastlogon"][0].ToString()));
                        DateTime pwdLastSetDateTime = DateTime.FromFileTime(long.Parse(entry.Attributes["pwdlastset"][0].ToString()));
                        Console.WriteLine($"Nombre de usuario: {usernameToSearch} \nNombre: {displayName} \nCorreo electrónico: {email} \n" +
                                          $"Whenchanged: {whenchanged} \nDelivery: {physicaldeliveryofficename} \nCreated: {whencreated} \n" +
                                          $"Lastlogon: {lastlogon} \nLastlogoff: {lastlogoff} \nLastSetPassword: {pwdLastSetDateTime}");
                        return true;
                    }
                }
                else
                {
                    // El usuario no se encontró
                    Console.WriteLine($"No se encontró el usuario: {usernameToSearch}");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error de LDAP: " + e.Message);
                return false;
            }
        }
        return false;
    }
    

    public bool AuthenticateUser(string _username, string _password)
    {
        const int ldapErrorInvalidCredentials = 0x31;
        try
        {
            using (var ldapConnection = new LdapConnection(_serverLDAP.serverPath))
            {
                var networkCredential = new NetworkCredential(_username, _password);
                ldapConnection.AuthType = AuthType.Basic;
                ldapConnection.Bind(networkCredential);
            }

            // If the bind succeeds, the credentials are valid
            return true;
        }
        catch (LdapException ldapException)
        {
            //Invalid credentials throw an exception with a specific error code
            if (ldapException.ErrorCode.Equals(ldapErrorInvalidCredentials))
            {            
                return false;
            }
            throw;
        }
    }
}