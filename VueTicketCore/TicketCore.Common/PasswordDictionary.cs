using System.Collections.Generic;

namespace TicketCore.Common
{
    public static class PasswordDictionary
    {
        private static readonly List<string> Credentials = new List<string>();

        static PasswordDictionary()
        {
            //Pass@123
            Credentials.Add("b6bc7b58510319a151d168ba3d5aecb3ac0a9708d06dd930f37fbc89b6cdc697");

            //Pass@1234
            Credentials.Add("1d95a4c6d681ede5b18c89b21ceb46bfea7b8e4d8f824107615a2ee297493710");

            //Pass@12345
            Credentials.Add("162fdbc2f46bbce7bf045ed3201873bdc6344857864544dfa35bf3d02e0ad5ba");

            //Pass@123456
            Credentials.Add("a62c5850c7fcdfe4cde5bcaeb12ea700812ef7caef8126e880a4b4ca68982498");

            //Pass@1234567
            Credentials.Add("0a88a89c97c84e983c0a44c7819e74f601e5407e390c98637316bf9557fd9dfb");

            //Pass@12345678
            Credentials.Add("a9605a150724172cf86cb759ed6bc51fd75b375ac38e55456a184ad17d2d9edf");

            //Pass@123456789
            Credentials.Add("eecf8aaff1afa9865af8c9edbc8770134fd9bb765d8843e3a798489146ca4ca5");

            //1q2w3e
            Credentials.Add("c0c4a69b17a7955ac230bfc8db4a123eaa956ccf3c0022e68b8d4e2f5b699d1f");

            //Pass$123
            Credentials.Add("2c4711b80b94707bf83819900169b7c3616d730e0c85c0d06a4a72afadec1e9f");

            //Pass#123
            Credentials.Add("2b005cc8b610fa5899a9f9e592671bba9776a0e778c7f88db9b54eef48490e94");

        }

        public static bool CheckCommonPassword(string passwordhash)
        {
            var contains = Credentials.Contains(passwordhash);
            return contains;
        }
    }
}