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
    using (LdapConnection cn = new LdapConnection(new LdapDirectoryIdentifier(_serverLDAP.serverPath,_serverLDAP.ServerPort)))
    {
        cn.SessionOptions.ProtocolVersion = 3;
        cn.AuthType = AuthType.Basic;
        try
        {
            cn.Bind(new NetworkCredential(_serverLDAP.userDN, _serverLDAP.userDNPassword));
            
            
            string searchFilter = String.Format("sAMAccountname={0}", usernameToSearch);
            string[] attributesToRetrieve = new string[] { "displayName", "mail", "whenchanged", "badpwdcount", "badpasswordtime", "whencreated", "lastlogoff", "physicaldeliveryofficename", "lastlogon", "pwdlastset" };
            
            SearchRequest searchRequest = new SearchRequest(
                _serverLDAP.distinguishedNames,
                searchFilter,
                SearchScope.Subtree,
                attributesToRetrieve
            );

            var searchResponse = (SearchResponse)cn.SendRequest(searchRequest);
             if (searchResponse.Entries.Count > 0)
             {
                 return true;
             }
            return false;
        }
        catch (LdapException lex)
        {
            Console.WriteLine($"Error LDAP: {lex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error general: {ex.Message}");
            return false;
        }
    }
}

private void HandleSearchResponse(string usernameToSearch, SearchResponse searchResponse)
{
    if (searchResponse.Entries.Count > 0)
    {
        foreach (SearchResultEntry entry in searchResponse.Entries)
        {
            DateTime lastlogon = DateTime.FromFileTime(long.Parse(entry.Attributes["lastlogon"][0].ToString()));
            DateTime pwdLastSetDateTime = DateTime.FromFileTime(long.Parse(entry.Attributes["pwdlastset"][0].ToString()));
            Console.WriteLine($"Nombre de usuario: {usernameToSearch}");
            Console.WriteLine($"Lastlogon: {lastlogon}");
            Console.WriteLine($"LastSetPassword: {pwdLastSetDateTime}");
        }
    }
    else
    {
        Console.WriteLine($"No se encontró el usuario: {usernameToSearch}");
    }
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