using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSPBAL;
using System.Web;
using System.Net.NetworkInformation;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlClient;
using System.Transactions;
using System.Text.RegularExpressions;
namespace FSPBAL
{
    /* public class PartialClass
     {*/
    public partial class General
    {

        public static Guid ID
        {
            get
            {
                //String temp = Guid.NewGuid().ToString().Substring(0, 8);
                return Guid.NewGuid();//int.Parse(temp, System.Globalization.NumberStyles.HexNumber).ToString();
            }
        }
        public static bool ValidateEmail(string Email)
        {
            //Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            Match match = regex.Match(Email);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //
       

        public static bool ValidateWebURL(string URL)
        {
            Regex regex = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            Match match = regex.Match(URL);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool ValidateSpace(string Value)
        {
            if (Value.Trim() != "")
            {
                Regex regex = new Regex(@"^[^\s]+$");
                Match match = regex.Match(Value);
                if (match.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public static string RemoveSpace(string Value)
        {
            string updateValue = string.Empty;
            if (Value.Trim() != "")
            {
                Regex regex = new Regex(@"^[^\s]+$");
                Match match = regex.Match(Value);
                if (match.Success)
                {
                    updateValue = Value;
                }
                else
                {
                    updateValue = Value.Replace(" ", "");
                }
            }
            return updateValue;
        }
        public static string ReplaceSingleQuote(string Value)
        {
            string updateValue = string.Empty;
            if (Value.Trim() != "")
            {
                updateValue = Value.Replace("'", "''");
            }
            return updateValue;
        }
        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string GetStatusMatrixValue(string Status)
        {
            try
            {
                string getvalue = string.Empty;
                string[,] StatusMatrix = new string[6, 2] { { "NEW", "APRV,REJD,STPD,CANC" }, { "PAPR", "APRV,REJD,STPD" }, { "STPD", "APRV,REJD,PAPR" }, { "REJD", "REOP,APRV" }, { "REOP", "APRV,REJD,STPD" }, { "CANC", "REOP" } };

                for (int i = 0; i < 6; i++)
                {
                    string bol = StatusMatrix[i, 0];
                    if (bol == Status)
                    {
                        getvalue = StatusMatrix[i, 1];
                    }
                }
                return getvalue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string GetPurchaseOrderStatusMatrix(string Status)
        {
            try
            {
                string getvalue = string.Empty;
                string[,] StatusMatrix = new string[4, 2] { { "DRAFT", "APRV,CANC" }, { "APRV", "CANC" }, { "REAPR", "APRV,CANC" }, { "CANC", "REAPR" } };

                for (int i = 0; i < 4; i++)
                {
                    string bol = StatusMatrix[i, 0];
                    if (bol == Status)
                    {
                        getvalue = StatusMatrix[i, 1];
                    }
                }
                return getvalue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string GetSupplierStatusMatrixValue(string Status)
        {
            try
            {
                string getvalue = string.Empty;
                string[,] StatusMatrix = new string[3, 2] { { "ACT", "UPRQD,BLKT" }, { "BLKT", "ACT" }, { "UPRQD", "ACT,BLKT" } };

                for (int i = 0; i < 3; i++)
                {
                    string bol = StatusMatrix[i, 0];
                    if (bol == Status)
                    {
                        getvalue = StatusMatrix[i, 1];
                    }
                }
                return getvalue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ValidatePassword(string password)
        {
            var input = password;

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("Password should not be empty");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{6,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            //if (!hasLowerChar.IsMatch(input))
            //{
            //    //ErrorMessage = "Password should contain At least one lower case letter";
            //    return false;
            //}
            if (!hasUpperChar.IsMatch(input))
            {
                //ErrorMessage = "Password should contain At least one upper case letter";
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                //ErrorMessage = "Password should not be less than or greater than 12 characters";
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                //ErrorMessage = "Password should contain At least one numeric value";
                return false;
            }

          /*  else if (!hasSymbols.IsMatch(input))
            {
                //ErrorMessage = "Password should contain At least one special case characters";
                return false;
            }*/
            else
            {
                return true;
            }
        }

        
        //Validate Dates
        public bool ValidateDates(string DateFrom, string DateTo, string FieldName)
        {
            try
            {
                if (DateFrom != "")
                {
                    if (DateFrom != null)
                    {
                        try
                        {
                            DateTime dt = DateTime.Parse(DateFrom);
                            return true;
                        }
                        catch (Exception ex)
                        {//1033
                            return false;
                        }
                    }
                }
                if (DateTo != "")
                {
                    if (DateTo != null)
                    {
                        try
                        {
                            DateTime dt = DateTime.Parse(DateTo); 
                            return true;
                        }
                        catch (Exception ex)
                        { 
                            return false;
                        }
                    }
                }
                if (DateFrom != "" && DateTo != "")
                {
                    if (DateTime.Parse(DateTo) < DateTime.Parse(DateFrom))
                    { 
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static string GetSupStatusMatrixValue(string Status, string UserName)
        {
            try
            {
                User usr = new User();
                string getvalue = string.Empty;
                string[, ,] StatusMatrix = new string[12, 2, 3] { { { "ACT", "WARNG", "SUP_BLKLIST_REQ" },  { "ACT", "UPRQD", "BUYER_ADMIN" } }, 
                                               { { "WARNG", "ACT", "SUP_BLKLIST_REQ" },  { "WARNG", "PBLKT", "SUP_BLKLIST_REQ" } },
                                               { { "PBLKT", "BLKT", "SUP_BLKLIST_APRV_L1" }, { "PBLKT", "WARNG", "SUP_BLKLIST_APRV_L1" } },
                                               { { "PBLKT", "BLKT", "SUP_BLKLIST_APRV_L2" }, { "PBLKT", "WARNG", "SUP_BLKLIST_APRV_L2" } },
                                               { { "BLKT", "PACT", "SUP_BLKLIST_REQ" }, { "BLKT", "PACT", "SUP_BLKLIST_REQ" } },
                                               { { "PACT", "BLKT", "SUP_BLKLIST_APRV_L1" }, { "PACT", "ACT", "SUP_BLKLIST_APRV_L1" } },
                                               { { "PACT", "BLKT", "SUP_BLKLIST_APRV_L2" }, { "PACT", "ACT", "SUP_BLKLIST_APRV_L2" } }, 
                                               { { "UPRQD", "ACT", "BUYER_ADMIN" }, { "UPRQD", "WARNG", "SUP_BLKLIST_REQ" } },
                                                { { "ACT", "WARNG", "BUYER_ADMIN" },  { "UPRQD", "WARNG", "BUYER_ADMIN" }},
                                                { { "ACT", "INACT", "FSP_ADMIN" },  { "UPRQD", "INACT", "FSP_ADMIN" }},
                                                 { { "UPRQD", "INACT", "FSP_ADMIN" },{ "ACT", "INACT", "FSP_ADMIN" }},
                                               // { { "ACT", "INACT", "SUP_PRFL_CR_APRV" },  { "UPRQD", "INACT", "SUP_PRFL_CR_APRV" }},
                 { { "WARNG", "ACT", "BUYER_ADMIN" },  { "WARNG", "ACT", "BUYER_ADMIN" }}};
                /*  for (int i = 0; i < 6; i++)
                    {*/
                int count = 0;
                for (int i = 0; i < StatusMatrix.GetLength(2); i++)
                {
                    for (int y = 0; y < StatusMatrix.GetLength(1); y++)
                    {
                        for (int x = 0; x < StatusMatrix.GetLength(0); x++)
                        {
                            string bol = StatusMatrix[x, 0, 0];
                            string Rol = StatusMatrix[x, y, 2];

                            if (bol == Status)
                            {
                                bool IsExist = usr.IsExistRole(Rol, UserName);
                                if (IsExist)
                                {
                                    if (count == 0)
                                    {
                                        getvalue = StatusMatrix[x, y, 1];
                                        count++;
                                    }
                                    else
                                    {
                                        getvalue += "," + StatusMatrix[x, y, 1];
                                    }
                                }
                            }

                        }
                    }
                }
                return getvalue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string SendMail(string Email, string Subject, string Memo)
        {
            try
            {
                string ProfileName = ConfigurationManager.AppSettings["ProfileName"];
                string ConnectionString = ConfigurationManager.ConnectionStrings["CS1"].ToString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand("msdb.dbo.sp_send_dbmail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                cmd.Parameters.Add("@profile_name", SqlDbType.VarChar).Value = ProfileName;
                cmd.Parameters.Add("@recipients", SqlDbType.VarChar).Value = Email;
                cmd.Parameters.Add("@subject", SqlDbType.VarChar).Value = Subject;
                cmd.Parameters.Add("@body_format", SqlDbType.VarChar).Value = "HTML";
                cmd.Parameters.Add("@body", SqlDbType.VarChar).Value = Memo;
                cmd.ExecuteNonQuery(); conn.Close();
                return "Mail has been send";
            }
            catch (Exception ex)
            {
                //lblError.Text = "Mail Sending" + ex.Message;
                return ex.Message;
            }///@from_address = 'custom display name <custom_address@your_domain.com>'
        }
        public static string SendMailFrom(string Email, string Subject, string Memo, string EmailFrom)
        {
            try
            {
                string ProfileName = ConfigurationManager.AppSettings["ProfileName"];
                string ConnectionString = ConfigurationManager.ConnectionStrings["CS1"].ToString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand("msdb.dbo.sp_send_dbmail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                cmd.Parameters.Add("@profile_name", SqlDbType.VarChar).Value = ProfileName;
                cmd.Parameters.Add("@recipients", SqlDbType.VarChar).Value = Email;
                cmd.Parameters.Add("@from_address", SqlDbType.VarChar).Value = EmailFrom;
                cmd.Parameters.Add("@subject", SqlDbType.VarChar).Value = Subject;
                cmd.Parameters.Add("@body_format", SqlDbType.VarChar).Value = "HTML";
                cmd.Parameters.Add("@body", SqlDbType.VarChar).Value = Memo;
                cmd.ExecuteNonQuery(); conn.Close();
                return "Mail has been send";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static bool VerifyMonthName(string MonthName)
        {

            if (MonthName == "Jan" || MonthName == "JAN" || MonthName == "jan")
            {
                return true;
            }
            else if (MonthName == "Feb" || MonthName == "FEB" || MonthName == "feb")
            {
                return true;
            }
            else if (MonthName == "Mar" || MonthName == "MAR" || MonthName == "mar")
            {
                return true;
            }
            else if (MonthName == "Apr" || MonthName == "APR" || MonthName == "apr")
            {
                return true;
            }
            else if (MonthName == "May" || MonthName == "MAY" || MonthName == "may")
            {
                return true;
            }
            else if (MonthName == "Jun" || MonthName == "JUN" || MonthName == "jun")
            {
                return true;
            }
            else if (MonthName == "Jul" || MonthName == "JUL" || MonthName == "jul")
            {
                return true;
            }
            else if (MonthName == "Aug" || MonthName == "AUG" || MonthName == "aug")
            {
                return true;
            }
            else if (MonthName == "Sep" || MonthName == "SEP" || MonthName == "sep")
            {
                return true;
            }
            else if (MonthName == "Oct" || MonthName == "OCT" || MonthName == "oct")
            {
                return true;
            }
            else if (MonthName == "Nov" || MonthName == "NOV" || MonthName == "nov")
            {
                return true;
            }
            else if (MonthName == "Dec" || MonthName == "DEC" || MonthName == "dec")
            {
                return true;
            }
            return false;
        }
        public static bool VerifyDate(string MonthName, string Year, string usrInput)
        {
            int MonthNumber = 0;
            if (MonthName == "Jan" || MonthName == "JAN" || MonthName == "jan")
            {
                MonthNumber = 1;
                // return true;
            }
            else if (MonthName == "Feb" || MonthName == "FEB" || MonthName == "feb")
            {
                MonthNumber = 2;
                //return true;
            }
            else if (MonthName == "Mar" || MonthName == "MAR" || MonthName == "mar")
            {
                MonthNumber = 3;
                //return true;
            }
            else if (MonthName == "Apr" || MonthName == "APR" || MonthName == "apr")
            {
                MonthNumber = 4;
                //return true;
            }
            else if (MonthName == "May" || MonthName == "MAY" || MonthName == "may")
            {
                MonthNumber = 5;
                // return true;
            }
            else if (MonthName == "Jun" || MonthName == "JUN" || MonthName == "jun")
            {
                MonthNumber = 6;
                // return true;
            }
            else if (MonthName == "Jul" || MonthName == "JUL" || MonthName == "jul")
            {
                MonthNumber = 7;
                // return true;
            }
            else if (MonthName == "Aug" || MonthName == "AUG" || MonthName == "aug")
            {
                MonthNumber = 8;
                // return true;
            }
            else if (MonthName == "Sep" || MonthName == "SEP" || MonthName == "sep")
            {
                MonthNumber = 9;
                //  return true;
            }
            else if (MonthName == "Oct" || MonthName == "OCT" || MonthName == "oct")
            {
                MonthNumber = 10;
                //return true;
            }
            else if (MonthName == "Nov" || MonthName == "NOV" || MonthName == "nov")
            {
                MonthNumber = 11;
                // return true;
            }
            else if (MonthName == "Dec" || MonthName == "DEC" || MonthName == "dec")
            {
                MonthNumber = 12;
                //return true;
            }
            int TotalDays = DateTime.DaysInMonth(int.Parse(Year), MonthNumber);
            if (int.Parse(usrInput) <= TotalDays)
            {
                return true;
            }
            return false;
        }
        public static string CheckFileName(string FileName1)
        {
            if (FileName1.Contains('#'))
            {
                FileName1 = FileName1.Replace('#', '-');
            }
            if (FileName1.Contains('@'))
            {
                FileName1 = FileName1.Replace('@', '-');
            }
            if (FileName1.Contains('$'))
            {
                FileName1 = FileName1.Replace('$', '-');
            }
            if (FileName1.Contains('^'))
            {
                FileName1 = FileName1.Replace('^', '-');
            }
            if (FileName1.Contains('%'))
            {
                FileName1 = FileName1.Replace('%', '-');
            }
            if (FileName1.Contains('&'))
            {
                FileName1 = FileName1.Replace('&', '-');
            }
            if (FileName1.Contains('*'))
            {
                FileName1 = FileName1.Replace('*', '-');
            }
            if (FileName1.Contains('('))
            {
                FileName1 = FileName1.Replace('(', '-');
            }
            if (FileName1.Contains(')'))
            {
                FileName1 = FileName1.Replace(')', '-');
            }
            return FileName1;
        }
        public static bool ValidateUploadFile(byte[] DocumentByte)
        {
            System.UInt32 mimeType;
            FindMimeFromData(0, null, DocumentByte, 256, null, 0, out mimeType, 0);
            System.IntPtr mimeTypePtr = new IntPtr(mimeType);
            string mime = Marshal.PtrToStringUni(mimeTypePtr);
            Marshal.FreeCoTaskMem(mimeTypePtr);
            if (mime == "application/x-msdownload" || mime == "application/octet-stream")
            {
                return false;
            }
            return true;
        }
        public static bool CheckFileExtension(string Extension)
        {
            if (Extension == ".PDF")
            {
                return true;
            }
            else if (Extension == ".DOC")
            {
                return true;
            }
            else if (Extension == ".DOCX")
            {
                return true;
            }
            else if (Extension == ".XLS")
            {
                return true;
            }
            else if (Extension == ".XLSX")
            {
                return true;
            }
            else if (Extension == ".CSV")
            {
                return true;
            }
            /* else if (Extension == ".ZIP")
             {
                 return true;
             }*/
            else if (Extension == ".JPG")
            {
                return true;
            }
            else if (Extension == ".JPEG")
            {
                return true;
            }
            else if (Extension == ".PNG")
            {
                return true;
            }
            else if (Extension == ".GIF")
            {
                return true;
            }
            else if (Extension == ".BMP")
            {
                return true;
            }
            else if (Extension == ".GIF")
            {
                return true;
            }

            return false;
        }

        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static System.UInt32 FindMimeFromData(
            System.UInt32 pBC,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            System.UInt32 cbSize,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
            System.UInt32 dwMimeFlags,
            out System.UInt32 ppwzMimeOut,
            System.UInt32 dwReserverd
        );
    }

    #region UserSession
    public partial class UserSession
    {
        public string getMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }


        //string CS = ConfigurationManager.ConnectionStrings["CS"].ToString();
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public bool SignIn(HttpRequest Request, string UserName)
        {
            try
            {
                //UserName;

                UserSession UsrSession = new UserSession();

                UsrSession.IPAddress = Request.UserHostAddress;
                UsrSession.LogonTime = DateTime.Now;
                UsrSession.MacAddress = getMacAddress();
                UsrSession.UserID = UserName;
                UsrSession.Status = 1;
                UsrSession.SessionID = Request.Cookies["ASP.NET_SessionID"].Value;
                db.UserSessions.InsertOnSubmit(UsrSession);
                db.SubmitChanges();
                return true;

            }
            catch (Exception ex)
            {

                ///// FM_ErrorLog ErrorLog = new FM_ErrorLog();
                //  ErrorLog.SaveError("Default", "Login", UserName, ex.Message);
                return false;
            }
            finally
            {
            }
        }
        public void SignOut(string SessionID, string UserName)
        {
            try
            {

                UserSession UsrSession = new UserSession();
                UsrSession = db.UserSessions.SingleOrDefault(x => x.UserID == Security.DecryptText(UserName) && x.SessionID == SessionID && x.Status == 1);
                if (UsrSession != null)
                {
                    //UsrSession.LastLogOutIPAddress = HttpContext.Current.Request.UserHostAddress;
                    // UsrSession.MacAddress = getMacAddress();
                    UsrSession.LogoutTime = DateTime.Now;
                    UsrSession.Status = 3;
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {

                /*    FM_ErrorLog ErrorLog = new FM_ErrorLog();
                    ErrorLog.SaveError("LogOut", "Signout", UserName, ex.Message);*/
                //return false;
            }
            finally
            {
            }
        }

        public static string GetUserIdentity(string UserIdentity)
        {
            try
            {

                string[] strArr = UserIdentity.Split(new char[] { '\\' });
                if (strArr.GetLength(0) > 1)
                {
                    return strArr[1];
                }
                else
                    return UserIdentity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string GetUserIdentity(HttpRequest Request)
        {
            try
            {
                string userid = "";
                if (!HttpContext.Current.User.Identity.IsAuthenticated) // Gets a value that indicates whether the user has been authenticated - namespace of "System.Web.Security;"
                {
                    return userid;
                }
                if (Request.QueryString["user"] == null)
                {
                    userid = GetUserIdentity(HttpContext.Current.User.Identity.Name);
                }
                else
                {
                    userid = Request.QueryString["user"];
                }
                return userid;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    #endregion

    #region Notification
    public partial class Notification
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public static string CalculatetimeDifference(string UserInput)
        {
            if (UserInput == "")
            {
                // return null;
            }
            else
            {
                DateTime d = DateTime.Parse(UserInput);
                TimeSpan s = DateTime.Now.Subtract(d);
                int dayDiff = (int)s.TotalDays;
                int secDiff = (int)s.TotalSeconds;
                /* if (dayDiff < 0 || dayDiff >= 31)
                 {
                     return null;
                 }*/
                if (dayDiff == 0)
                {
                    if (secDiff < 60)
                    {
                        return "just now";
                    }
                    else if (secDiff < 120)
                    {
                        return "1 minute ago";
                    }
                    else if (secDiff < 3600)
                    {
                        return string.Format("{0} minutes ago",
                            Math.Floor((double)secDiff / 60));
                    }
                    else if (secDiff < 7200)
                    {
                        return "1 hour ago";
                    }
                    else if (secDiff < 86400)
                    {
                        return string.Format("{0} hours ago",
                            Math.Floor((double)secDiff / 3600));
                    }
                }
                else if (dayDiff == 1)
                {
                    return "yesterday";
                }
                else if (dayDiff < 7)
                {
                    return string.Format("{0} days ago",
                    dayDiff);
                }
                else if (dayDiff < 31)
                {
                    return string.Format("{0} weeks ago",
                    Math.Ceiling((double)dayDiff / 7));
                }
                else if (dayDiff < 365)
                {
                    return string.Format("{0} months ago", Math.Ceiling((double)dayDiff / 30));
                }
                else if (dayDiff > 366)
                {
                    return string.Format("{0} year ago", Math.Ceiling((double)dayDiff / 365));
                }
            }
            return null;
        }
        public string SendNotificationSupplier(string Email, string Subject, string Body, int TempID, string UserID, bool IsNotificationSend)
        {
            Notification noti = new Notification();
            noti.Subject = Subject;
            if (TempID == -1)
            {
                noti.NotificationTemplatesID = null;
            }
            else
            {
                noti.NotificationTemplatesID = TempID;
            }
            noti.Body = Body;
            noti.Sender = "noreply@fibrexholding.com";
            noti.Recepient = Email;
            noti.UserID = UserID;
            noti.SendDateTime = DateTime.Now;
            noti.IsRead = false;
            db.Notifications.InsertOnSubmit(noti);
            db.SubmitChanges();
            if (IsNotificationSend == true)
            {
                General.SendMail(Email, Subject, Body);
            }
            return true.ToString();

        }
        public string SendNotificationSupplierSenderFrom(string Email, string Subject, string Body, int TempID, string UserID, bool IsNotificationSend, string SenderFrom)
        {
            Notification noti = new Notification();
            noti.Subject = Subject;
            if (TempID == -1 || TempID == 0)
            {
                noti.NotificationTemplatesID = null;
            }
            else
            {
                noti.NotificationTemplatesID = TempID;
            }
            noti.Body = Body;
            noti.Sender = SenderFrom;
            noti.Recepient = Email;
            noti.UserID = UserID;
            noti.SendDateTime = DateTime.Now;
            noti.IsRead = false;
            db.Notifications.InsertOnSubmit(noti);
            db.SubmitChanges();
            if (IsNotificationSend == true)
            {
                General.SendMailFrom(Email, Subject, Body, SenderFrom);
            }
            return true.ToString();
        }
    }

    #endregion


    #region ChangeRequest
    public partial class ChangeRequest
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public int VerifyChangeRequest(string SupplierID, string CurrentValue, string ProposedValue, string TableName, string FieldName, string Action, string RecordID, string UserName)
        {
            ChangeRequest CR;
            ChangeRequestDetail CRD;
            int ChangeRequestIDs = 0;
            CR = db.ChangeRequests.SingleOrDefault(x => x.SupplierID == int.Parse(SupplierID) && x.Status == "PAPR");
            if (CR != null)
            {
                ChangeRequestIDs = CR.ChangeRequestID;
            }
            else
            {
                CR = new ChangeRequest();
                CR.SupplierID = int.Parse(SupplierID);
                CR.Memo = null;
                CR.Status = "PAPR";
                CR.CreatedBy = UserName;
                CR.CreationDateTime = DateTime.Now;
                db.ChangeRequests.InsertOnSubmit(CR);
                db.SubmitChanges();
                CR = db.ChangeRequests.SingleOrDefault(x => x.SupplierID == int.Parse(SupplierID) && x.Status == "PAPR");
                if (CR != null)
                {
                    ChangeRequestIDs = CR.ChangeRequestID;
                }
            }

            CRD = db.ChangeRequestDetails.SingleOrDefault(x => x.FieldName == FieldName && x.ChangeRequestID == ChangeRequestIDs);
            if (CRD != null)
            {
                return 1;
            }
            else
            {
                CRD = new ChangeRequestDetail();
                CRD.ActionTaken = Action;
                CRD.CreatedBy = UserName;
                CRD.CreationDateTime = DateTime.Now;
                if (RecordID != "")
                {
                    CRD.RecordID = int.Parse(RecordID);
                }
                CRD.ChangeRequestID = ChangeRequestIDs;
                if (CurrentValue != "")
                {
                    CRD.CurrentValue = CurrentValue;
                }
                else
                {
                    CRD.CurrentValue = null;
                }
                CRD.ProposedValue = ProposedValue;
                CRD.TableName = TableName;
                CRD.FieldName = FieldName;
                db.ChangeRequestDetails.InsertOnSubmit(CRD);
                db.SubmitChanges();
                return 1;
            }
        }
    }
    #endregion

    #region STG_FIRMS_SUPPLIER
    public partial class STG_FIRMS_SUPPLIER
    {
        User usr = new User();
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string SaveStgFirms(string TranscationType, int UsrSupplierID, string Status, DateTime StatusDateTime, string SupplierName, string SupplierShortName, string SupType, string Country, string BusClass, string OfficialEmail,
           string ContactFirstPerson, string ContactLastPerson, string ContactPosition, string ContactMobile, string ContactPhone, string ContactExt, string RegDocType, string RegDocID, string RegDocIssAuth,
           string RegDocExpireDate, string AddressCountry, string AddressLine1, string AddressLine2, string AddressCity, string AddressPostalCode, string AddressPhoneNum, string AddressFaxNum, string CreatedBy, string OwnerName, string VATRegistrationNo, string IsVatRegistered, string RegistrationType, string VATGrpRepName)
        {
            try
            {
                //using (TransactionScope trans = new TransactionScope())
                // { 
                STG_FIRMS_SUPPLIER ObjSub;
                string TransEvent = string.Empty;
                /*if (TranscationType == "New")
                {
                    TransEvent = "Insert";
                }
                else
                {
                    TransEvent = "Update";
                }*/
                ObjSub = new STG_FIRMS_SUPPLIER();
                ObjSub.STG_FIRMS_SUPPLIER_ID = General.ID;
                /***Transaction Information**/

                ObjSub.TransEvent = TranscationType;
                ObjSub.TransStatus = "New";
                ObjSub.TransDateTime = DateTime.Now;
                ObjSub.TransStatusDateTime = DateTime.Now;
                /****/
                ObjSub.SupplierID = UsrSupplierID;
                ObjSub.SupplierStatus = Status;
                if (SupplierShortName != "")
                {
                    ObjSub.SupplierShortName = SupplierShortName;
                }
                ObjSub.SupplierStatusDateTime = StatusDateTime;
                ObjSub.SupplierName = SupplierName;
                ObjSub.SupplierType = SupType;
                ObjSub.Country = Country;
                ObjSub.BusinessClass = BusClass;
                ObjSub.OwnerName = OwnerName;
                ObjSub.OfficialEmail = OfficialEmail;
                //if (vatgroupNo != "")
                //{
                //    ObjSub.VATGroupNo = vatgroupNo;
                //}
                if (VATRegistrationNo != "")
                {
                    ObjSub.VATRegistrationNo = VATRegistrationNo;
                }
                ObjSub.ContactPerson = ContactFirstPerson + " " + ContactLastPerson;
                //ObjSub.ContactLastName = ContactLastPerson;
                ObjSub.ContactPosition = ContactPosition;
                ObjSub.ContactMobile = ContactMobile;
                ObjSub.ContactPhone = ContactPhone;
                if (ContactExt != "")
                {
                    ObjSub.ContactExt = ContactExt;
                }
                ObjSub.RegDocType = RegDocType;
                ObjSub.RegDocID = RegDocID;
                ObjSub.RegDocIssAuth = RegDocIssAuth;
                if (RegDocExpireDate != "")
                {
                    ObjSub.RegDocExpiryDate = DateTime.Parse(RegDocExpireDate);
                }
                ObjSub.AddressCountry = AddressCountry;
                ObjSub.AddressLine1 = AddressLine1;
                if (AddressLine2 != "")
                {
                    ObjSub.AddressLine2 = AddressLine2;
                }
                ObjSub.AddressCity = AddressCity;
                ObjSub.AddressPostalCode = AddressPostalCode;
                ObjSub.AddressPhoneNum = AddressPhoneNum;
                if (AddressFaxNum != "")
                {
                    ObjSub.AddressFaxNum = AddressFaxNum;
                }
                string GetName = string.Empty;
                if (CreatedBy == "Guest")
                {
                    GetName = "(Self-Service) External User";
                }
                else
                {
                    if (CreatedBy == "sysadmin")
                    {
                        GetName = "sysadmin";
                    }
                    else
                    {
                        GetName = usr.GetFullName(CreatedBy);
                    }
                }
                if (IsVatRegistered != "Select")
                {
                    //if (IsVatRegistered == "true")
                    //{
                    //    ObjSub.IsVATRegistered = true;
                    //}
                    //if (IsVatRegistered == "1")
                    //{
                    //    ObjSub.IsVATRegistered = false;
                    //}
                    if (IsVatRegistered == "true" || IsVatRegistered == "True")
                    {
                        ObjSub.IsVATRegistered = true;
                    }
                    if (IsVatRegistered == "false" || IsVatRegistered == "False")
                    {
                        ObjSub.IsVATRegistered = false;
                    }
                }
                if (RegistrationType != "Select")
                {
                    ObjSub.VATRegistrationType = RegistrationType;
                }
                if (VATGrpRepName != "")
                {
                    ObjSub.VATGrpRepName = VATGrpRepName;
                }
                ObjSub.RegisteredBy = GetName;
                db.STG_FIRMS_SUPPLIERs.InsertOnSubmit(ObjSub);
                db.SubmitChanges();
                //trans.Complete();
                return "Success";
                //}
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public string SaveSTGTableForIMS(string TranscationType, int UsrSupplierID, string Status, DateTime StatusDateTime)
        {
            try
            {
                //using (TransactionScope trans = new TransactionScope())
                // { 
                Supplier Sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == UsrSupplierID);
                var SupAddres = db.SupplierAddresses.Where(x => x.SupplierID == Sup.SupplierID).OrderBy(x => x.SupplierAddressID).Take(1);
                Registration reg = db.Registrations.SingleOrDefault(x => x.RegistrationID == Sup.RegistrationNo);
                foreach (var a in SupAddres)
                {
                    if (Sup != null)
                    {
                        STG_FIRMS_SUPPLIER ObjSub;
                        string TransEvent = string.Empty;
                        ObjSub = new STG_FIRMS_SUPPLIER();
                        ObjSub.STG_FIRMS_SUPPLIER_ID = General.ID;
                        /***Transaction Information**/

                        ObjSub.TransEvent = TranscationType;
                        ObjSub.TransStatus = "New";
                        ObjSub.TransDateTime = DateTime.Now;
                        ObjSub.TransStatusDateTime = DateTime.Now;
                        ObjSub.SupplierID = UsrSupplierID;
                        ObjSub.SupplierStatus = Sup.Status;
                        ObjSub.SupplierShortName = Sup.SupplierShortName;
                        ObjSub.SupplierStatusDateTime = StatusDateTime;
                        ObjSub.SupplierName = Sup.SupplierName;
                        ObjSub.SupplierType = Sup.SupplierType;
                        ObjSub.Country = Sup.Country;
                        ObjSub.BusinessClass = Sup.BusinessClass;
                        ObjSub.OwnerName = Sup.OwnerName;
                        ObjSub.OfficialEmail = Sup.OfficialEmail;
                        ObjSub.VATRegistrationNo = Sup.VATRegistrationNo;
                        ObjSub.ContactPerson = Sup.ContactFirstName + " " + Sup.ContactLastName;
                        ObjSub.ContactPosition = Sup.ContactPosition;
                        ObjSub.ContactMobile = Sup.ContactMobile;
                        ObjSub.ContactPhone = Sup.ContactPhone;
                        ObjSub.ContactExt = Sup.ContactExtension;
                        ObjSub.RegDocType = Sup.RegDocType;
                        ObjSub.RegDocID = Sup.RegDocID;
                        ObjSub.RegDocIssAuth = Sup.RegDocIssAuth;
                        ObjSub.RegDocExpiryDate = Sup.RegDocExpiryDate;
                        ObjSub.AddressCountry = a.Country;
                        ObjSub.AddressLine1 = a.AddressLine1;
                        ObjSub.AddressLine2 = a.AddressLine2;

                        ObjSub.AddressCity = a.City;
                        ObjSub.AddressPostalCode = a.PostalCode;
                        ObjSub.AddressPhoneNum = a.PhoneNum;
                        ObjSub.AddressFaxNum = a.FaxNum;

                        string GetName = string.Empty;
                        string CreatedBy = reg.CreatedBy;
                        if (CreatedBy == "Guest")
                        {
                            GetName = "(Self-Service) External User";
                        }
                        else
                        {
                            if (CreatedBy == "sysadmin")
                            {
                                GetName = "sysadmin";
                            }
                            else
                            {
                                GetName = usr.GetFullName(CreatedBy);
                            }
                        }
                        ObjSub.IsVATRegistered = Sup.IsVATRegistered;
                        ObjSub.VATRegistrationType = Sup.VATRegistrationType;
                        ObjSub.VATGrpRepName = Sup.VATGrpRepName;
                        ObjSub.RegisteredBy = GetName;
                        db.STG_FIRMS_SUPPLIERs.InsertOnSubmit(ObjSub);
                        db.SubmitChanges();
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
    #endregion

    #region Supplier
    public partial class Supplier
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string GetSupplierID(Guid ID)
        {
            try
            {
                string SupID = string.Empty;
                FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
                Supplier Sup = db.Suppliers.SingleOrDefault(x => x.ID == ID);
                if (Sup == null)
                {
                    SupID = "Supplier not found";
                }
                else
                {
                    SupID = Sup.SupplierID.ToString();
                }
                return SupID;
            }
            catch (Exception ex)
            {
                return false.ToString();
            }
        }
        public string NewSupplierRegistration(string RegistrationID, string Status, string SupplierName, string SupplierShortName, string SupType, string Country, string BusClass, string OfficialEmail,
            string ContactFirstName, string ContactLastName, string ContactPosition, string ContactMobile, string ContactPhone, string ContactExt, string RegDocType, string RegDocID, string RegDocIssAuth,
            string RegDocExpireDate, string UserName, string OldStatus, string Memo, string UsrStatus, string ownerName, string VatRegistrationNo, string IsCusVatRegistered, string VatRegistrationType, string VATGrpRepName)
        {

            string UserID = string.Empty;
            string newTempPass = string.Empty;
            try
            {
                Supplier ObjSub;
                SS_NumDomain SS_Num = new SS_NumDomain();
                using (TransactionScope trans = new TransactionScope())
                {
                    ObjSub = new Supplier();
                    ObjSub.ID = General.ID;
                    ObjSub.SupplierName = SupplierName;
                    if ((SupplierShortName != "") || (SupplierShortName != null))
                    {
                        ObjSub.SupplierShortName = SupplierShortName;
                    }
                    else
                    {
                        ObjSub.SupplierShortName = null;
                    }
                    ObjSub.SupplierType = SupType;
                    ObjSub.Country = Country;
                    ObjSub.Status = Status;
                    ObjSub.BusinessClass = BusClass;
                    ObjSub.ContactPosition = ContactPosition;
                    ObjSub.OfficialEmail = OfficialEmail;
                    ObjSub.ContactFirstName = ContactFirstName; // Last Name is pending
                    ObjSub.ContactLastName = ContactLastName;
                    ObjSub.OwnerName = ownerName;
                    if (RegDocExpireDate != "")
                    {
                        ObjSub.RegDocExpiryDate = DateTime.Parse(RegDocExpireDate);
                    }
                    ObjSub.ContactMobile = ContactMobile;
                    ObjSub.ContactPhone = ContactPhone;
                    if (ContactExt != "")
                    {
                        ObjSub.ContactExtension = ContactExt;
                    }
                    ObjSub.CreatedBy = UserName;
                    ObjSub.CreationDateTime = DateTime.Now;
                    if (RegDocType != "")
                    {
                        ObjSub.RegDocType = RegDocType;
                    }
                    ObjSub.RegistrationNo = int.Parse(RegistrationID);
                    if (RegDocID != "")
                    {
                        ObjSub.RegDocID = RegDocID;
                    }
                    if (RegDocIssAuth != "")
                    {
                        ObjSub.RegDocIssAuth = RegDocIssAuth;
                    }
                    //if (VatGroupNo != "")
                    //{
                    //    ObjSub.VATGroupNo = VatGroupNo;
                    //}
                    if (VatRegistrationNo != "")
                    {
                        ObjSub.VATRegistrationNo = VatRegistrationNo;
                    }
                    if (IsCusVatRegistered != "Select" || IsCusVatRegistered != "")
                    {
                        ObjSub.IsVATRegistered = bool.Parse(IsCusVatRegistered);
                    }
                    //if (IsCusVatRegistered != "Select")
                    //{
                    //    if (IsCusVatRegistered == "1")
                    //    {
                    //        ObjSub.IsVATRegistered = true;
                    //    }
                    //    else if (IsCusVatRegistered == "0")
                    //    {
                    //        ObjSub.IsVATRegistered = false;
                    //    }
                    //}
                    if (VatRegistrationType != "Select")
                    {
                        ObjSub.VATRegistrationType = VatRegistrationType;
                    }
                    if (VATGrpRepName != "")
                    {
                        ObjSub.VATGrpRepName = VATGrpRepName;
                    }
                    db.Suppliers.InsertOnSubmit(ObjSub);
                    db.SubmitChanges();

                    ObjSub = db.Suppliers.SingleOrDefault(x => x.RegistrationNo == int.Parse(RegistrationID));
                    if (ObjSub != null)
                    {
                        Registration Objreg = db.Registrations.SingleOrDefault(x => x.RegistrationID == int.Parse(RegistrationID));
                        Objreg.LastModifiedBy = UserName;
                        Objreg.LastModifiedDateTime = DateTime.Now;
                        Objreg.Status = "APRV";
                        db.SubmitChanges();

                        SupplierStatusHistory Suphistory = new SupplierStatusHistory();
                        Suphistory.Memo = Memo;
                        Suphistory.OldStatus = null;
                        Suphistory.NewStatus = Status;
                        Suphistory.SupplierID = ObjSub.SupplierID;
                        Suphistory.ModifiedBy = "sysadmin";
                        Suphistory.ModificationDateTime = DateTime.Now;
                        db.SupplierStatusHistories.InsertOnSubmit(Suphistory);
                        db.SubmitChanges();

                        //RegistrationStatusHistory
                        RegistrationStatusHistory Reghistory = new RegistrationStatusHistory();
                        Reghistory.Memo = Memo;
                        Reghistory.OldStatus = OldStatus;
                        Reghistory.NewStatus = "APRV";
                        Reghistory.RegistrationID = Objreg.RegistrationID;
                        Reghistory.ModifiedBy = UserName;
                        Reghistory.ModificationDateTime = DateTime.Now;
                        db.RegistrationStatusHistories.InsertOnSubmit(Reghistory);
                        db.SubmitChanges();
                        UserID = "AD_" + ObjSub.SupplierID;

                        SS_UserLoginActivity SSUsr;
                        SSUsr = db.SS_UserLoginActivities.SingleOrDefault(x => x.UserID == UserID);
                        if (SSUsr == null)
                        {
                            SSUsr = new SS_UserLoginActivity();
                            SSUsr.ChangePassOnFirstLogin = false;
                            SSUsr.UserID = UserID;
                            db.SS_UserLoginActivities.InsertOnSubmit(SSUsr);
                            db.SubmitChanges();
                        }
                        User usr = new User();
                        if (usr != null)
                        {
                            newTempPass = newRandowPassword();
                            usr.CreatedBy = UserName;
                            usr.CreationDateTime = DateTime.Now;
                            usr.Email = OfficialEmail;
                            usr.AuthSystem = "FSP";//Objreg.RegistrationType;
                            usr.FirstName = ContactFirstName;
                            usr.LastName = ContactLastName;
                            usr.Password = Security.EncryptText(newTempPass);
                            usr.PhoneNum = ContactPhone;
                            usr.Status = UsrStatus;
                            usr.UserID = UserID;
                            db.Users.InsertOnSubmit(usr);
                            db.SubmitChanges();
                        }

                        SupplierUser Spu = new SupplierUser();
                        Spu.UserID = UserID;
                        Spu.SupplierID = ObjSub.SupplierID;
                        Spu.CreatedBy = UserName;
                        Spu.CreationDateTime = DateTime.Now;
                        db.SupplierUsers.InsertOnSubmit(Spu);
                        db.SubmitChanges();

                        SupplierUser SupUsr = db.SupplierUsers.FirstOrDefault(x => x.SupplierID == ObjSub.SupplierID);
                        if (SupUsr != null)
                        {
                            A_User a_usr = new A_User();
                            a_usr.SaveRecordInAuditUsers(SupUsr.UserID, UserName, "New");
                        }

                        SS_UserSecurityGroup SecUsr = new SS_UserSecurityGroup();
                        SecUsr.UserID = UserID;
                        SecUsr.SecurityGroupID = 1;
                        SecUsr.CreatedBy = UserName;
                        SecUsr.CreationDateTime = DateTime.Now;
                        db.SS_UserSecurityGroups.InsertOnSubmit(SecUsr);
                        db.SubmitChanges();
                    }
                    /*ObjSub.RegDocExpireDate = DateTime.Parse(RegDocExpireDate);
                    ObjSub.AddressCountry = AddressCountry;
                    ObjSub.AddressLine1 = AddressLine1;
                    ObjSub.AddressLine2 = AddressLine2;
                    ObjSub.AddressCity = AddressCity;
                    ObjSub.AddressPostalCode = AddressPostalCode;
                    ObjSub.AddressPhoneNum = AddressPhoneNum;
                    ObjSub.AddressFaxNum = AddressFaxNum;*/
                    trans.Complete();
                }
                return ObjSub.SupplierID.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string newRandowPassword()
        {
            Random rand = new Random(1000001);
            int newpassword = rand.Next();

            return newpassword.ToString();
        }
        public SqlDataReader GetMaxlength(string TableName)
        {
            SqlConnection rConn = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ToString());
            SqlDataReader SqlDr;

            if (rConn.State == ConnectionState.Open)
            {
                rConn.Close();
            }

            SqlCommand SqlCmd = new SqlCommand("SELECT COLUMN_NAME, DATA_TYPE,CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + TableName + "' and  CHARACTER_MAXIMUM_LENGTH is not null", rConn);
            rConn.Open();
            SqlDr = SqlCmd.ExecuteReader();
            try
            {
                if (SqlDr.HasRows)
                {
                    return SqlDr;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
            return SqlDr;
        } 
        public int GetFieldMaxlength(string TableName, string FieldName)
        {
            string DataLength = string.Empty;
            SqlConnection rConn = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ToString());
            if (rConn.State == ConnectionState.Open)
            {
                rConn.Close();
            }
            SqlCommand SqlCmd = new SqlCommand("SELECT Character_maximum_length FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + TableName + "' and COLUMN_NAME='" + FieldName + "' AND CHARACTER_MAXIMUM_LENGTH is not null", rConn);
            rConn.Open();
            try
            {
                var Length = SqlCmd.ExecuteScalar();
                if (Length != null)
                {
                    DataLength = Length.ToString();
                }
                else
                {
                    DataLength = "0";
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                rConn.Close();
            }
            return int.Parse(DataLength);
        }
        public string getEmployeeStatusfromHistory(string SupplierID)
        {
            string Status = string.Empty;
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ToString());
            SqlDataAdapter daMyAdapter;
            DataSet dsMyDataSet;
            try
            {

                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }

                Conn.Open();
                daMyAdapter = new SqlDataAdapter("Select top (1) * from SupplierStatusHistory where SupplierID ='" + SupplierID + "' and NewStatus not in ('WARNG','BLKT','PBLKT','PACT') order by ModificationDateTime desc", Conn);
                dsMyDataSet = new DataSet();
                daMyAdapter.SelectCommand.CommandTimeout = 240;
                daMyAdapter.Fill(dsMyDataSet);
                if (dsMyDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsMyDataSet.Tables[0].Rows.Count; i++)
                    {
                        Status = dsMyDataSet.Tables[0].Rows[i]["NewStatus"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                Conn.Close();
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return Status;
        }
        public string getUserIDfromStatusHistory(string SupplierID, string OldStatus, string NewStatus)
        {
            string Name = string.Empty;
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ToString());
            SqlDataAdapter daMyAdapter;
            DataSet dsMyDataSet;
            try
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
                Conn.Open();
                daMyAdapter = new SqlDataAdapter("Select top (1) * from SupplierStatusHistory where SupplierID ='" + SupplierID + "' and OldStatus='" + OldStatus + "' and NewStatus='" + NewStatus + "' order by ModificationDateTime Desc", Conn);
                dsMyDataSet = new DataSet();
                daMyAdapter.SelectCommand.CommandTimeout = 240;
                daMyAdapter.Fill(dsMyDataSet);
                if (dsMyDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsMyDataSet.Tables[0].Rows.Count; i++)
                    {
                        Name = dsMyDataSet.Tables[0].Rows[i]["ModifiedBy"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Conn.Close();
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return Name;
        }
        public string getUserIDonStatus(string SupplierID, string Status)
        {
            string Name = string.Empty;
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ToString());
            SqlDataAdapter daMyAdapter;
            DataSet dsMyDataSet;
            try
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
                Conn.Open();
                daMyAdapter = new SqlDataAdapter("Select top (1) * from SupplierStatusHistory where SupplierID ='" + SupplierID + "' and NewStatus='" + Status + "' order by ModificationDateTime Desc", Conn);
                dsMyDataSet = new DataSet();
                daMyAdapter.SelectCommand.CommandTimeout = 240;
                daMyAdapter.Fill(dsMyDataSet);
                if (dsMyDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsMyDataSet.Tables[0].Rows.Count; i++)
                    {
                        Name = dsMyDataSet.Tables[0].Rows[i]["ModifiedBy"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Conn.Close();
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return Name;
        }
        public string GetSupplierName(int SupplierID)
        {
            try
            {
                string SupName = string.Empty;
                FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
                Supplier Sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == SupplierID);
                if (Sup == null)
                {
                    SupName = "Supplier not found";
                }
                else
                {
                    SupName = Sup.SupplierName.ToString();
                }
                return SupName;
            }
            catch (Exception ex)
            {
                return false.ToString();
            }
        }

        public string GetSupplierStatus(string CompanyID)
        {
            try
            {
                string SupStatus = string.Empty;
                FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
                Supplier Sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == int.Parse(CompanyID) && x.Status == "ACT");
                if (Sup != null)
                { 
                    SupStatus = Sup.Status.ToString();
                }
                return SupStatus;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool verifySupVatRegistrationNOWithRegistrationType(string VatRegistration, string SupplierID)
        {
            //Registration reg = new Registration();
            Supplier reg2;
            reg2 = (from regis in db.Suppliers
                    where regis.VATRegistrationNo == VatRegistration && regis.SupplierID != int.Parse(SupplierID) //&& regis.VATRegistrationType=="IND"
                    select regis).FirstOrDefault();
            if (reg2 != null)
            {
                return false;
                //}
            }
            return true;
        }
    }
    #endregion

    #region A_Supplier
    public partial class A_Supplier
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string SaveRecordInAudit(int SupplierID, string UserName, string Action)
        {
            try
            {
                A_Supplier A_Sup = new A_Supplier();
                Supplier Sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == SupplierID);
                if (Sup != null)
                {
                }
                A_Sup.AuditBy = UserName; // UserID
                A_Sup.AuditTimeStamp = DateTime.Now;
                A_Sup.AuditAction = Action;
                A_Sup.SupplierName = Sup.SupplierName;
                A_Sup.SupplierShortName = Sup.SupplierShortName;
                A_Sup.Country = Sup.Country;
                A_Sup.SupplierType = Sup.SupplierType;
                A_Sup.BusinessClassification = Sup.BusinessClass;
                A_Sup.OfficialEmail = Sup.OfficialEmail;
                A_Sup.ContactFirstName = Sup.ContactFirstName;
                A_Sup.ContactLastName = Sup.ContactLastName;
                A_Sup.ContactPhone = Sup.ContactPhone;
                A_Sup.ContactPosition = Sup.ContactPosition;
                A_Sup.ContactMobile = Sup.ContactMobile;
                A_Sup.ContactExtension = Sup.ContactExtension;
                // A_Sup.URL = Sup.URL;
                A_Sup.OwnerName = Sup.OwnerName;
                A_Sup.RegDocType = Sup.RegDocType;
                A_Sup.RegDocID = Sup.RegDocID;
                A_Sup.PaymentMethod = Sup.PaymentMethod;
                A_Sup.SupplierID = Sup.SupplierID;
                A_Sup.RegDocIssAuth = Sup.RegDocIssAuth;
                A_Sup.RegDocExpiryDate = Sup.RegDocExpiryDate;
                //A_Sup.VATGroupNo = Sup.VATGroupNo;
                A_Sup.VATRegistrationNo = Sup.VATRegistrationNo;
                A_Sup.VATRegistrationType = Sup.VATRegistrationType;
                A_Sup.IsVATRegistered = Sup.IsVATRegistered;
                A_Sup.VATGrpRepName = Sup.VATGrpRepName;
                A_Sup.Status = Sup.Status;
                db.A_Suppliers.InsertOnSubmit(A_Sup);
                db.SubmitChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
    #endregion


    #region SupplierAddress
    public partial class SupplierAddress
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string SaveNewSupplierAddress(string AddressName, string Country, string AddressLine1, string AddressLine2, string city, string postalCode, string PhoneNum, string FaxNum, string UserName, string SupplierID)
        {
            string warningMasg = string.Empty;
            A_SupplierAddress a_Supaddress = new A_SupplierAddress();
            try
            {
                int VerifyChanges = 0;
                SupplierAddress SupAddress = new SupplierAddress();
                if (SupAddress != null)
                {
                    if (AddressName != "")
                    {
                        SupAddress.AddressName = AddressName;
                        SupAddress.SupplierID = int.Parse(SupplierID);
                    }

                    if (Country != "Select")
                    {
                        VerifyChanges = 1;
                        SupAddress.Country = Country;
                    }
                    if (AddressLine1 != "")//&& regSup.AddressLine1 == null
                    {
                        VerifyChanges = 1;
                        SupAddress.AddressLine1 = AddressLine1;
                    }
                    if (AddressLine2 != "")
                    {
                        VerifyChanges = 1;
                        SupAddress.AddressLine2 = AddressLine2;
                    }

                    if (city != "")
                    {
                        VerifyChanges = 1;
                        SupAddress.City = city;
                    }

                    if (postalCode != "")
                    {
                        VerifyChanges = 1;
                        SupAddress.PostalCode = postalCode;
                    }
                    if (PhoneNum != "")
                    {
                        VerifyChanges = 1;
                        SupAddress.PhoneNum = PhoneNum;
                    }
                    if (FaxNum != "")
                    {
                        VerifyChanges = 1;
                        SupAddress.FaxNum = FaxNum;
                    }
                }
                if (VerifyChanges == 1)
                {
                    SupAddress.CreatedBy = UserName;
                    SupAddress.CreationDateTime = DateTime.Now;
                    db.SupplierAddresses.InsertOnSubmit(SupAddress);
                    db.SubmitChanges();
                    SupplierAddress regSup = db.SupplierAddresses.SingleOrDefault(x => x.SupplierID == int.Parse(SupplierID) && x.AddressName == AddressName);
                    if (regSup != null)
                    {
                        a_Supaddress.SaveRecordInAuditSupplierAddress(regSup.SupplierAddressID, UserName, "New");
                    }
                    warningMasg = "Success";
                }
                else
                {
                    warningMasg = "nochange";
                }
                return warningMasg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string UpdateSupplierAddress(string AddressID, string Country, string AddressLine1, string AddressLine2, string city, string postalCode, string PhoneNum, string FaxNum, string UserName)
        {
            string warningMasg = string.Empty;
            A_SupplierAddress a_Supaddress = new A_SupplierAddress();
            try
            {
                int CheckUpdateProfileValue = 0;
                SupplierAddress regSup = db.SupplierAddresses.SingleOrDefault(x => x.SupplierAddressID == int.Parse(AddressID));
                if (regSup != null)
                {
                    if (Country != "Select")
                    {
                        if (regSup.Country != Country)
                        {
                            CheckUpdateProfileValue = 1;
                            regSup.Country = Country;
                        }
                    }
                    if ((AddressLine1 != "" && regSup.AddressLine1 == null) || (AddressLine1 != "" && regSup.AddressLine1 != null))
                    {
                        if (regSup.AddressLine1 != AddressLine1)
                        {
                            CheckUpdateProfileValue = 1;
                            regSup.AddressLine1 = AddressLine1;
                        }
                    }

                    if ((AddressLine2 != "" && regSup.AddressLine2 == null) || (AddressLine2 != "" && regSup.AddressLine2 != null))
                    {
                        if (regSup.AddressLine2 != AddressLine2)
                        {
                            CheckUpdateProfileValue = 1;
                            regSup.AddressLine2 = AddressLine2;
                        }
                    }

                    if ((city != "" && regSup.City == null) || (city != "" && regSup.City != null))
                    {
                        if (regSup.City != city)
                        {
                            CheckUpdateProfileValue = 1;
                            regSup.City = city;
                        }
                    }

                    if ((postalCode != "" && regSup.PostalCode == null) || (postalCode != "" && regSup.PostalCode != null))
                    {
                        if (regSup.PostalCode != postalCode)
                        {
                            CheckUpdateProfileValue = 1;
                            regSup.PostalCode = postalCode;
                        }
                    }
                    if ((PhoneNum != "" && regSup.PhoneNum == null) || (PhoneNum != "" && regSup.PhoneNum != null))
                    {
                        if (regSup.PhoneNum != PhoneNum)
                        {
                            CheckUpdateProfileValue = 1;
                            regSup.PhoneNum = PhoneNum;
                        }
                    }

                    if ((FaxNum != "" && regSup.FaxNum == null) || (FaxNum != "" && regSup.FaxNum != null))
                    {
                        if (regSup.FaxNum != FaxNum)
                        {
                            CheckUpdateProfileValue = 1;
                            regSup.FaxNum = FaxNum;
                        }
                    }
                    if (CheckUpdateProfileValue == 1)
                    {
                        regSup.LastModifiedBy = UserName;
                        regSup.LastModifiedDateTime = DateTime.Now;
                        db.SubmitChanges();
                        a_Supaddress.SaveRecordInAuditSupplierAddress(int.Parse(AddressID), UserName, "Update");
                        warningMasg = "Success";
                    }
                    else
                    {
                        warningMasg = "nochange";
                    }
                }
                return warningMasg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
    #endregion

    #region A_SupplierAddress
    public partial class A_SupplierAddress
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string SaveRecordInAuditSupplierAddress(int AddressID, string UserName, string Action)
        {
            try
            {
                A_SupplierAddress A_SupAddress = new A_SupplierAddress();
                SupplierAddress SupAddress = db.SupplierAddresses.SingleOrDefault(x => x.SupplierAddressID == AddressID);
                if (SupAddress != null)
                {
                    A_SupAddress.SupplierAddressID = SupAddress.SupplierAddressID;
                    A_SupAddress.SupplierID = SupAddress.SupplierID;
                    A_SupAddress.AddressName = SupAddress.AddressName;
                    A_SupAddress.Country = SupAddress.Country;
                    A_SupAddress.AddressLine1 = SupAddress.AddressLine1;
                    A_SupAddress.AddressLine2 = SupAddress.AddressLine2;
                    A_SupAddress.City = SupAddress.City;
                    A_SupAddress.PostalCode = SupAddress.PostalCode;
                    A_SupAddress.PhoneNum = SupAddress.PhoneNum;
                    A_SupAddress.FaxNum = SupAddress.FaxNum;
                    A_SupAddress.AuditBy = UserName;
                    A_SupAddress.AuditTimeStamp = DateTime.Now;
                    A_SupAddress.AuditAction = Action;
                }
                db.A_SupplierAddresses.InsertOnSubmit(A_SupAddress);
                db.SubmitChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
    #endregion

    #region A_SupplierBankingDetail
    public partial class A_SupplierBankingDetail
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public void SaveRecordInAuditSupplierBank(int BankID, string UserName, string Action)
        {
            try
            {
                A_SupplierBankingDetail A_SupBank = new A_SupplierBankingDetail();
                SupplierBankingDetail SupBank = db.SupplierBankingDetails.SingleOrDefault(x => x.SupplierBankDetailID == BankID);
                if (SupBank != null)
                {
                    A_SupBank.SupplierBankDetailID = SupBank.SupplierBankDetailID;
                    A_SupBank.SupplierID = SupBank.SupplierID;
                    A_SupBank.BankName = SupBank.BankName;
                    A_SupBank.BranchAddress = SupBank.BranchAddress;
                    A_SupBank.AccountName = SupBank.AccountName;
                    A_SupBank.IBAN = SupBank.IBAN;
                    A_SupBank.AccountNum = SupBank.AccountNum;
                    A_SupBank.Country = SupBank.Country;
                    A_SupBank.AuditBy = UserName;
                    A_SupBank.AuditTimeStamp = DateTime.Now;
                    A_SupBank.AuditAction = Action;
                }
                db.A_SupplierBankingDetails.InsertOnSubmit(A_SupBank);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }
        }
    }
    #endregion


    #region SupplierBankingDetail
    public partial class SupplierBankingDetail
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string SaveNewBankInformation(string AccountName, string AccountNum, string BankName, string BankAddress, string BankCountry, string Iban, string UserName, string SupplierID)
        {
            A_SupplierBankingDetail a_bank = new A_SupplierBankingDetail();
            string warningMasg = string.Empty;
            int CheckUpdateProfileValue = 0;
            try
            {
                SupplierBankingDetail Bank = new SupplierBankingDetail();
                if (Bank != null)
                {
                    if (AccountName != "")
                    {
                        CheckUpdateProfileValue = 1;
                        Bank.AccountName = AccountName;
                    }
                    if (AccountNum != "")
                    {
                        CheckUpdateProfileValue = 1;
                        Bank.AccountNum = AccountNum;
                    }
                    Bank.SupplierID = int.Parse(SupplierID);
                    if (BankName != "")
                    {
                        CheckUpdateProfileValue = 1;
                        Bank.BankName = BankName;
                    }
                    if (BankAddress != "")
                    {
                        CheckUpdateProfileValue = 1;
                        Bank.BranchAddress = BankAddress;
                    }
                    if (BankCountry != "Select")
                    {
                        CheckUpdateProfileValue = 1;
                        Bank.Country = BankCountry;
                    }
                    if (Iban != "")
                    {
                        CheckUpdateProfileValue = 1;
                        Bank.IBAN = Iban;
                    }
                    if (CheckUpdateProfileValue == 1)
                    {
                        Bank.CreatedBy = UserName;
                        Bank.CreationDateTime = DateTime.Now;
                        db.SupplierBankingDetails.InsertOnSubmit(Bank);
                        db.SubmitChanges();
                        SupplierBankingDetail bk = db.SupplierBankingDetails.FirstOrDefault(x => x.SupplierID == int.Parse(SupplierID));
                        if (bk != null)
                        {
                            a_bank.SaveRecordInAuditSupplierBank(bk.SupplierBankDetailID, UserName, "New");
                        }
                        warningMasg = "Success";
                    }
                    else
                    {
                        warningMasg = "nochange";
                    }
                }
                return warningMasg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string UpdateBankInformation(string BankID, string AccountName, string AccountNum, string BankName, string BankAddress, string BankCountry, string Iban, string UserName)
        {
            A_SupplierBankingDetail a_bank = new A_SupplierBankingDetail();
            string warningMasg = string.Empty;
            int CheckUpdateProfileValue = 0;
            try
            {
                SupplierBankingDetail Bank = db.SupplierBankingDetails.SingleOrDefault(x => x.SupplierBankDetailID == int.Parse(BankID));
                if (Bank != null)
                {

                    if ((AccountName != "" && Bank.AccountName == null) || (AccountName != "" && Bank.AccountName != null))
                    {
                        if (Bank.AccountName != AccountName)
                        {
                            CheckUpdateProfileValue = 1;
                            Bank.AccountName = AccountName;
                        }
                    }
                    else if (AccountName == "" && Bank.AccountName != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Bank.AccountName = null;
                    }

                    if ((AccountNum != "" && Bank.AccountNum == null) || (AccountNum != "" && Bank.AccountNum != null))
                    {
                        if (Bank.AccountNum != AccountNum)
                        {
                            CheckUpdateProfileValue = 1;
                            Bank.AccountNum = AccountNum;
                        }
                    }
                    else if (AccountNum == "" && Bank.AccountName != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Bank.AccountNum = null;
                    }

                    if ((BankName != "" && Bank.BankName == null) || (BankName != "" && Bank.BankName != null))
                    {
                        if (Bank.BankName != BankName)
                        {
                            CheckUpdateProfileValue = 1;
                            Bank.BankName = BankName;
                        }
                    }
                    else if (BankName == "" && Bank.BankName != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Bank.BankName = null;
                    }
                    if ((BankAddress != "" && Bank.BranchAddress == null) || (BankAddress != "" && Bank.BranchAddress != null))
                    {
                        if (Bank.BranchAddress != BankAddress)
                        {
                            CheckUpdateProfileValue = 1;
                            Bank.BranchAddress = BankAddress;
                        }
                    }
                    else if (BankAddress == "" && Bank.BranchAddress != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Bank.BranchAddress = null;
                    }

                    if (BankCountry != "Select")
                    {
                        if (Bank.Country != BankCountry)
                        {
                            CheckUpdateProfileValue = 1;
                            Bank.Country = BankCountry;
                        }
                    }
                    if ((Iban != "" && Bank.IBAN == null) || (Iban != "" && Bank.IBAN != null))
                    {
                        if (Bank.IBAN != Iban)
                        {
                            CheckUpdateProfileValue = 1;
                            Bank.IBAN = Iban;
                        }
                    }
                    else if (Iban == "" && Bank.IBAN != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Bank.IBAN = null;
                    }
                    if (CheckUpdateProfileValue == 1)
                    {
                        Bank.LastModifiedBy = UserName;
                        Bank.LastModifiedDateTime = DateTime.Now;
                        db.SubmitChanges();
                        a_bank.SaveRecordInAuditSupplierBank(Bank.SupplierBankDetailID, UserName, "Update");
                        warningMasg = "Success";
                    }
                    else
                    {
                        warningMasg = "nochange";
                    }
                }
                return warningMasg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
    #endregion

    #region A_Registration
    public partial class A_Registration
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string SaveRecordRegInAudit(int RegistrationID, string UserName, string Action)
        {
            try
            {
                A_Registration A_Sup = new A_Registration();
                Registration Sup = db.Registrations.SingleOrDefault(x => x.RegistrationID == RegistrationID);
                if (Sup != null)
                {
                }
                A_Sup.AuditBy = UserName; // UserID
                A_Sup.AuditTimeStamp = DateTime.Now;
                A_Sup.AuditAction = Action;
                A_Sup.SupplierName = Sup.SupplierName;
                A_Sup.SupplierShortName = Sup.SupplierShortName;
                A_Sup.Country = Sup.Country;
                A_Sup.SupplierType = Sup.SupplierType;
                A_Sup.BusinessClass = Sup.BusinessClass;
                A_Sup.OfficialEmail = Sup.OfficialEmail;
                A_Sup.ContactFirstName = Sup.ContactFirstName;
                A_Sup.ContactLastName = Sup.ContactLastName;
                A_Sup.RegistrationType = Sup.RegistrationType;
                A_Sup.OwnerName = Sup.OwnerName;
                A_Sup.ContactPhone = Sup.ContactPhone;
                A_Sup.ContactPosition = Sup.ContactPosition;
                A_Sup.ContactMobile = Sup.ContactMobile;
                A_Sup.ContactExtension = Sup.ContactExtension;
                A_Sup.RegDocType = Sup.RegDocType;
                A_Sup.RegDocID = Sup.RegDocID;
                A_Sup.RegistrationID = Sup.RegistrationID;
                A_Sup.RegDocIssAuth = Sup.RegDocIssAuth;
                A_Sup.RegDocExpiryDate = Sup.RegDocExpiryDate;
                //A_Sup.VATGroupNo = Sup.VATGroupNo;
                A_Sup.VATRegistrationNo = Sup.VATRegistrationNo;
                A_Sup.VATRegistrationType = Sup.VATRegistrationType;
                A_Sup.IsVATRegistered = Sup.IsVATRegistered;
                A_Sup.VATGrpRepName = Sup.VATGrpRepName;
                A_Sup.Status = Sup.Status;
                db.A_Registrations.InsertOnSubmit(A_Sup);
                db.SubmitChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
    #endregion

    #region A_RegSupplierAddress
    public partial class A_RegSupplierAddress
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public void SaveRecordInAuditRegSupplierAddress(int AddressID, string UserName, string Action)
        {
            try
            {
                A_RegSupplierAddress A_SupAddress = new A_RegSupplierAddress();
                RegSupplierAddress SupAddress = db.RegSupplierAddresses.SingleOrDefault(x => x.RegAddressID == AddressID);
                if (SupAddress != null)
                {
                    A_SupAddress.RegAddressID = SupAddress.RegAddressID;
                    A_SupAddress.RegistrationID = SupAddress.RegistrationID;
                    A_SupAddress.AddressName = SupAddress.AddressName;
                    A_SupAddress.Country = SupAddress.Country;
                    A_SupAddress.AddressLine1 = SupAddress.AddressLine1;
                    A_SupAddress.AddressLine2 = SupAddress.AddressLine2;
                    A_SupAddress.City = SupAddress.City;
                    A_SupAddress.PostalCode = SupAddress.PostalCode;
                    A_SupAddress.PhoneNum = SupAddress.PhoneNum;
                    A_SupAddress.FaxNum = SupAddress.FaxNum;
                    A_SupAddress.AuditBy = UserName;
                    A_SupAddress.AuditAction = Action;
                    A_SupAddress.AuditTimeStamp = DateTime.Now;
                }
                db.A_RegSupplierAddresses.InsertOnSubmit(A_SupAddress);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }
        }
    }
    #endregion

    #region Registration
    public partial class Registration
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public void SendExternalRegistrationNotification(string StatusCode, string OldStatus, string RegID, string Memo, string UserName, string UserEmail, string UseID)
        {
            string Subject = string.Empty;
            int SenderFlag = 0;
            string Body = string.Empty;
            string Subject1 = string.Empty;
            string Body1 = string.Empty;
            string Email = string.Empty;
            int notifyTmpID = 0;
            bool IsSendNotification = false;
            NotificationTemplate NotifyTemp;
            NotificationTemplate NotifyTemp1;
            Notification notify = new Notification();
            Registration Objreg = db.Registrations.SingleOrDefault(x => x.RegistrationID == int.Parse(RegID));
            User usr = db.Users.FirstOrDefault(x => x.UserID == UserName);
            if (StatusCode == "REJD")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "PRSP_SUP_REG_REJD");
                notifyTmpID = NotifyTemp.NotificationTemplatesID;
                IsSendNotification = bool.Parse(NotifyTemp.IsNotificationSend.ToString());
                if (NotifyTemp != null)
                {
                    Body = NotifyTemp.Body.Replace("{RegNo}", RegID).Replace("{StatusComment}", " " + Memo);
                    Subject = NotifyTemp.Subject.Replace("{RegNo}", RegID);
                }
                SenderFlag = 1;
            }
            else if (StatusCode == "STPD")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "PRSP_SUP_REG_STPD");
                notifyTmpID = NotifyTemp.NotificationTemplatesID;
                IsSendNotification = bool.Parse(NotifyTemp.IsNotificationSend.ToString());
                if (NotifyTemp != null)
                {
                    Body = NotifyTemp.Body.Replace("{RegNo}", RegID).Replace("{StatusComment}", " " + Memo);
                    Subject = NotifyTemp.Subject.Replace("{RegNo}", RegID);
                }
                SenderFlag = 1;
            }
            else if (StatusCode == "REOP")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "PRSP_SUP_REG_REOP");
                notifyTmpID = NotifyTemp.NotificationTemplatesID;
                IsSendNotification = bool.Parse(NotifyTemp.IsNotificationSend.ToString());
                if (NotifyTemp != null)
                {
                    Body = NotifyTemp.Body.Replace("{RegNo}", RegID);
                    Subject = NotifyTemp.Subject.Replace("{RegNo}", RegID);
                }

                SenderFlag = 1;
                NotifyTemp1 = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "PRSP_SUP_REG_REOPEN_ASMT");
                if (NotifyTemp1 != null)
                {
                    Subject1 = NotifyTemp1.Subject.Replace("{RegNo}", RegID);
                    Body1 = NotifyTemp1.Body.Replace("{RegNo}", RegID).Replace("{Reopened.FirstName}", usr.FirstName).Replace("{ReopenedBy.LASTNAME}", usr.LastName).Replace("{RegID}", Security.URLEncrypt(RegID)).Replace("{sName}", Security.URLEncrypt(Objreg.SupplierName));
                }

                //PRSP_SUP_REG_REOPEN_ASMT
                List<SS_UserSecurityGroup> grp = db.SS_UserSecurityGroups.Where(x => x.SecurityGroupID == 4).ToList();
                if (grp.Count > 0)
                {
                    foreach (var ab in grp)
                    {
                        User usr1 = db.Users.FirstOrDefault(x => x.UserID == ab.UserID);
                        if (usr1 != null)
                        {
                            notify.SendNotificationSupplierSenderFrom(usr1.Email, Subject1, Body1, NotifyTemp1.NotificationTemplatesID, usr1.UserID, bool.Parse(NotifyTemp1.IsNotificationSend.ToString()), "noreply@fibrexholding.com");
                        }
                    }
                }


            }
            else if (StatusCode == "PAPR")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_REG_PAPR");
                notifyTmpID = NotifyTemp.NotificationTemplatesID;
                IsSendNotification = bool.Parse(NotifyTemp.IsNotificationSend.ToString());
                if (NotifyTemp != null)
                {
                    Body = NotifyTemp.Body.Replace("{RegNo}", RegID).Replace("{ids}", Security.URLEncrypt(RegID)).Replace("{sname}", Security.URLEncrypt((Objreg.SupplierName))).Replace("{RegID}", Security.URLEncrypt(RegID));
                    Subject = NotifyTemp.Subject.Replace("{RegNo}", RegID);
                }
            }
            if (Objreg != null)
            {
                Objreg.LastModifiedBy = UserName;
                Objreg.Status = StatusCode;
                Objreg.LastModifiedDateTime = DateTime.Now;
                //Objreg.StatusComment = Memo;
                db.SubmitChanges();
                if (Objreg.RegistrationType == "INT")
                {
                    Email = usr.Email;
                }
                else
                {
                    Email = UserEmail;
                }


                RegistrationStatusHistory Reghistory = new RegistrationStatusHistory();
                Reghistory.Memo = Memo;
                Reghistory.OldStatus = OldStatus;
                Reghistory.NewStatus = StatusCode;
                Reghistory.ModifiedBy = UserName;
                Reghistory.ModificationDateTime = DateTime.Now;
                Reghistory.RegistrationID = Objreg.RegistrationID;
                db.RegistrationStatusHistories.InsertOnSubmit(Reghistory);
                db.SubmitChanges();

                if (SenderFlag == 1)
                {
                    notify.SendNotificationSupplierSenderFrom(Email, Subject, Body, notifyTmpID, UseID, IsSendNotification, "registration@fibrex.ae");
                }
                else
                {
                    notify.SendNotificationSupplier(Email, Subject, Body, notifyTmpID, UseID, IsSendNotification);
                }
            }
        }
        public void SendInternalRegistrationNotification(string StatusCode, string OldStatus, string RegID, string Memo, string UserName, string UserEmail, string UserID)
        {
            string Subject = string.Empty;
            string Body = string.Empty;
            string Subject1 = string.Empty;
            string Body1 = string.Empty;
            int notifyTmpID = 0;
            string Email = string.Empty;
            bool isNotificationSend = false;
            NotificationTemplate NotifyTemp;
            NotificationTemplate NotifyTemp1;
            Notification notify = new Notification();
            Registration Objreg = db.Registrations.SingleOrDefault(x => x.RegistrationID == int.Parse(RegID));
            User usr = db.Users.SingleOrDefault(x => x.UserID == UserName);
            if (StatusCode == "REJD")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "INT_SUP_REQ_REJD");
                notifyTmpID = NotifyTemp.NotificationTemplatesID;
                isNotificationSend = bool.Parse(NotifyTemp.IsNotificationSend.ToString());
                if (NotifyTemp != null)
                {
                    Body = NotifyTemp.Body.Replace("{RegNo}", RegID).Replace("{StatusComment}", " " + Memo);
                    Subject = NotifyTemp.Subject.Replace("{RegNo}", RegID);
                }
            }
            else if (StatusCode == "STPD")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "INT_SUP_REQ_STPD");
                notifyTmpID = NotifyTemp.NotificationTemplatesID;
                isNotificationSend = bool.Parse(NotifyTemp.IsNotificationSend.ToString());
                if (NotifyTemp != null)
                {
                    Body = NotifyTemp.Body.Replace("{RegNo}", RegID).Replace("{StatusComment}", " " + Memo);
                    Subject = NotifyTemp.Subject.Replace("{RegNo}", RegID);
                }
            }
            else if (StatusCode == "REOP")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "INT_SUP_REQ_REOP");
                notifyTmpID = NotifyTemp.NotificationTemplatesID;
                isNotificationSend = bool.Parse(NotifyTemp.IsNotificationSend.ToString());
                if (NotifyTemp != null)
                {
                    Body = NotifyTemp.Body.Replace("{RegNo}", RegID);
                    Subject = NotifyTemp.Subject.Replace("{RegNo}", RegID);
                }
                NotifyTemp1 = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "INT_SUP_REQ_REOPEN_ASMT");
                if (NotifyTemp1 != null)
                {
                    Subject1 = NotifyTemp1.Subject.Replace("{RegNo}", RegID);
                    Body1 = NotifyTemp1.Body.Replace("{RegNo}", RegID).Replace("{Reopened.FirstName}", usr.FirstName).Replace("{ReopenedBy.LASTNAME}", usr.LastName).Replace("{RegID}", Security.URLEncrypt(RegID)).Replace("{sName}", Security.URLEncrypt(Objreg.SupplierName));
                }

                //PRSP_SUP_REG_REOPEN_ASMT
                List<SS_UserSecurityGroup> grp = db.SS_UserSecurityGroups.Where(x => x.SecurityGroupID == 4).ToList();
                if (grp.Count > 0)
                {
                    foreach (var ab in grp)
                    {
                        User usr1 = db.Users.FirstOrDefault(x => x.UserID == ab.UserID);
                        if (usr1 != null)
                        {
                            notify.SendNotificationSupplier(usr1.Email, Subject1, Body1, NotifyTemp1.NotificationTemplatesID, ab.UserID, bool.Parse(NotifyTemp1.IsNotificationSend.ToString()));
                        }
                    }
                }
            }
            else if (StatusCode == "PAPR")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_REG_PAPR");
                isNotificationSend = bool.Parse(NotifyTemp.IsNotificationSend.ToString());
                if (NotifyTemp != null)
                {
                    Body1 = NotifyTemp.Body.Replace("{RegNo}", RegID).Replace("{ids}", Security.URLEncrypt(RegID)).Replace("{sname}", Security.URLEncrypt((Objreg.SupplierName))).Replace("{RegID}", Security.URLEncrypt(RegID));
                    Subject1 = NotifyTemp.Subject.Replace("{RegNo}", RegID);

                    List<SS_UserSecurityGroup> grp = db.SS_UserSecurityGroups.Where(x => x.SecurityGroupID == 4).ToList();
                    if (grp.Count > 0)
                    {
                        foreach (var ab in grp)
                        {
                            User usr1 = db.Users.FirstOrDefault(x => x.UserID == ab.UserID);
                            if (usr1 != null)
                            {
                                notify.SendNotificationSupplier(usr1.Email, Subject1, Body1, NotifyTemp.NotificationTemplatesID, ab.UserID, bool.Parse(NotifyTemp.IsNotificationSend.ToString()));
                            }
                        }
                    }
                }
            }

            if (Objreg != null)
            {
                Objreg.LastModifiedBy = UserName;
                Objreg.Status = StatusCode;
                Objreg.LastModifiedDateTime = DateTime.Now;
                db.SubmitChanges();
                if (Objreg.RegistrationType == "INT")
                {
                    Email = UserEmail;
                }
                else
                {
                    Email = usr.Email;
                }
                RegistrationStatusHistory Reghistory = new RegistrationStatusHistory();
                Reghistory.Memo = Memo;
                Reghistory.OldStatus = OldStatus;
                Reghistory.ModifiedBy = UserName;
                Reghistory.NewStatus = StatusCode;
                Reghistory.ModificationDateTime = DateTime.Now;
                Reghistory.RegistrationID = Objreg.RegistrationID;
                db.RegistrationStatusHistories.InsertOnSubmit(Reghistory);
                db.SubmitChanges();
            }
            if (Subject != "" && Body != "")
            {
                notify.SendNotificationSupplier(Email, Subject, Body, notifyTmpID, UserID, isNotificationSend);
            }
        }
    }
    #endregion

    #region A_User
    public partial class A_User
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public void SaveRecordInAuditUsers(string userID, string UserName, string Action)
        {
            try
            {
                A_User A_usr = new A_User();
                User usr = db.Users.SingleOrDefault(x => x.UserID == userID);
                if (usr != null)
                {
                    A_usr.AuthSystem = usr.AuthSystem;
                    A_usr.AuditAction = Action;
                    A_usr.AuditBy = UserName;
                    A_usr.UserID = userID;
                    A_usr.AuditTimeStamp = DateTime.Now;
                    A_usr.Email = usr.Email;
                    A_usr.FirstName = usr.FirstName;
                    A_usr.LastName = usr.LastName;
                    A_usr.Password = usr.Password;
                    A_usr.PhoneNum = usr.PhoneNum;
                    A_usr.Status = usr.Status;
                    A_usr.Title = usr.Title;
                }
                db.A_Users.InsertOnSubmit(A_usr);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }
        }
    }
    #endregion

    #region A_Attachment
    public partial class A_Attachment
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string SaveRecordInAuditAttachment(int AttachmentID, string UserName, string Action)
        {
            try
            {
                A_Attachment A_attach = new A_Attachment();
                Attachment ObjAttach = db.Attachments.FirstOrDefault(x => x.AttachmentID == AttachmentID);
                if (ObjAttach != null)
                {
                    A_attach.AttachmentID = ObjAttach.AttachmentID;
                    A_attach.AuditAction = Action;
                    A_attach.AuditBy = UserName;
                    A_attach.AuditTimeStamp = DateTime.Now;
                    A_attach.Description = ObjAttach.Description;
                    A_attach.FileExtension = ObjAttach.FileExtension;
                    A_attach.FileName = ObjAttach.FileName;
                    A_attach.FileSize = ObjAttach.FileSize;
                    A_attach.FileURL = ObjAttach.FileURL;
                    A_attach.Status = ObjAttach.Status;
                    A_attach.Title = ObjAttach.Title;
                    A_attach.OwnerID = ObjAttach.OwnerID;
                    A_attach.OwnerTable = ObjAttach.OwnerTable;
                }
                db.A_Attachments.InsertOnSubmit(A_attach);
                db.SubmitChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
    #endregion

    #region A_UserSecurityGroup
    public partial class A_UserSecurityGroup
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public void SaveRecordInAuditUsers(string userID, int SecurityGroupID, string UserName, string Action)
        {
            try
            {
                A_UserSecurityGroup A_usrsec = new A_UserSecurityGroup();
                SS_UserSecurityGroup usrsec = db.SS_UserSecurityGroups.SingleOrDefault(x => x.UserID == userID && x.SecurityGroupID == SecurityGroupID);
                if (usrsec != null)
                {
                    A_usrsec.UserID = usrsec.UserID;
                    A_usrsec.SecurityGroupID = usrsec.SecurityGroupID;
                    A_usrsec.AuditAction = Action;
                    A_usrsec.AuditBy = UserName;
                    A_usrsec.AuditTimeStamp = DateTime.Now;
                    A_usrsec.SecurityGroupMemberID = usrsec.SecurityGroupMemberID;
                }
                db.A_UserSecurityGroups.InsertOnSubmit(A_usrsec);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }
        }

        //mms
    }
    #endregion

    #region SS_Message
    public partial class SS_Message
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string getMsgDetail(int MessageID)
        {
            SS_Message msg = db.SS_Messages.FirstOrDefault(x => x.MessageID == MessageID);
            if (msg != null)
            {
                return msg.Value;
            }
            return "Error";
        }
        public string GetMessageBg(int mesgID)
        {
            SS_Message msg = db.SS_Messages.FirstOrDefault(x => x.MessageID == mesgID);
            string MsgClas = string.Empty;
            if (msg.MessageType == "ERROR")
            {
                MsgClas = "alert alert-danger alert-dismissable";
            }
            else if (msg.MessageType == "AFFIRM")
            {
                MsgClas = "alert alert-success alert-dismissable";
            }
            else if (msg.MessageType == "ALERT")
            {
                MsgClas = "alert alert-warning alert-dismissable";
            }
            return MsgClas;
        }
    }
    #endregion

    #region User
    public partial class User
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string GetFullName(string userID)
        {
            string Name = string.Empty;
            User usr = db.Users.SingleOrDefault(x => x.UserID == userID);
            if (usr != null)
            {
                char[] FirstName = usr.FirstName.ToCharArray();
                FirstName[0] = char.ToUpper(FirstName[0]);
                char[] LastName = usr.LastName.ToCharArray();
                LastName[0] = char.ToUpper(LastName[0]);
                //return new string(a);

                Name = char.ToUpper(FirstName[0]) + usr.FirstName.Substring(1) + " " + (char.ToUpper(LastName[0])) + usr.LastName.Substring(1);
                //Name = usr.FirstName.ToUpper() + ' ' + ;
            }
            return Name;
        }
        public string GetSupplierUserID(string SupplierID)
        {
            string UsrID = string.Empty;
            SupplierUser usr = db.SupplierUsers.FirstOrDefault(x => x.SupplierID == int.Parse(SupplierID));
            if (usr != null)
            {
                UsrID = usr.UserID;
            }
            return UsrID;
        }
        public string GetSupplierEmail(int SupplierID)
        {
            string Email = string.Empty;
            Supplier sup = db.Suppliers.FirstOrDefault(x => x.SupplierID == SupplierID);
            if (sup != null)
            {
                Email = sup.OfficialEmail;
            }
            return Email;
        }
        public string GetUserEmail(string UserName)
        {
            string Email = string.Empty;
            User usr = db.Users.FirstOrDefault(x => x.UserID == UserName);
            if (usr != null)
            {
                Email = usr.Email;
            }
            return Email;
        }
        public bool IsExistRole(string RoleName, string UserName)
        {
            try
            {
                string value = string.Empty;
                SS_SecurityGroup usrRole = db.SS_SecurityGroups.FirstOrDefault(x => x.SecurityGroupName == RoleName);
                if (usrRole != null)
                {
                    SS_UserSecurityGroup usr = db.SS_UserSecurityGroups.FirstOrDefault(x => x.SecurityGroupID == usrRole.SecurityGroupID && x.UserID == UserName);
                    if (usr != null)
                    {
                        value = "true";
                    }
                    else
                    {
                        value = "false";
                    }
                }
                return bool.Parse(value);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void ChangeStatusSupplierUser(int SupplierID, string UserName, string Status)
        {
            SupplierUser SupUsr = db.SupplierUsers.SingleOrDefault(x => x.SupplierID == SupplierID);
            if (SupUsr != null)
            {
                User usr = db.Users.SingleOrDefault(x => x.UserID == SupUsr.UserID);
                if (usr != null)
                {
                    usr.Status = Status;
                    usr.LastModifiedBy = UserName;
                    usr.LastModifiedDateTime = DateTime.Now;
                    db.SubmitChanges();
                    A_User A_usr = new A_User();
                    A_usr.SaveRecordInAuditUsers(SupUsr.UserID, UserName, "Update");
                }
            }
        }
    }
    #endregion

    #region SupplierUser
    public partial class SupplierUser
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string GetUserID(int SupplierID)
        {
            string UserID = string.Empty;
            SupplierUser usr = db.SupplierUsers.FirstOrDefault(x => x.SupplierID == SupplierID);
            if (usr != null)
            {
                UserID = usr.UserID;
            }
            return UserID;
        }
    }
    #endregion

    #region A_POTEMPLATE
    public partial class A_POTEMPLATE
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string SaveRecordInAuditPOTemplates(int TemplateID, string UserName, string Action)
        {
            try
            {
                POTEMPLATE ObjTemp = db.POTEMPLATEs.SingleOrDefault(x => x.POTEMPLATEID == TemplateID);
                if (ObjTemp != null)
                {
                    A_POTEMPLATE ObjATemp = new A_POTEMPLATE();

                    ObjATemp.AUDITBY = UserName;
                    ObjATemp.AUDITACTION = Action;
                    ObjATemp.AUDITTIMESTAMP = DateTime.Now;
                    //
                    ObjATemp.POTEMPLATENAME = ObjTemp.POTEMPLATENAME;
                    ObjATemp.POTEMPLATEDESC = ObjTemp.POTEMPLATEDESC; 
                    ObjATemp.POTYPE = ObjTemp.POTYPE;
                    ObjATemp.PROJECTCODE = ObjTemp.PROJECTCODE;
                    ObjATemp.ORGCODE = ObjTemp.ORGCODE;
                    ObjATemp.PAYMENTTERMS = ObjTemp.PAYMENTTERMS;
                    ObjATemp.BUYERCODE = ObjTemp.BUYERCODE;
                    ObjATemp.BUYERNAME = ObjTemp.BUYERNAME;
                    ObjATemp.SHIPTOADDR = ObjTemp.SHIPTOADDR;
                    ObjATemp.SHIPTOATTN1MOB = ObjTemp.SHIPTOATTN1MOB;
                    ObjATemp.SHIPTOATTN1NAME = ObjTemp.SHIPTOATTN1NAME;
                    //ObjATemp.SHIPTOATTN1PHO = ObjTemp.SHIPTOATTN1PHO;
                    ObjATemp.SHIPTOATTN1POS = ObjTemp.SHIPTOATTN1POS;
                    ObjATemp.SHIPTOATTN2MOB = ObjTemp.SHIPTOATTN2MOB;
                    ObjATemp.SHIPTOATTN2NAME = ObjTemp.SHIPTOATTN2NAME;
                    //ObjATemp.SHIPTOATTN2PHO = ObjTemp.SHIPTOATTN2PHO;
                    ObjATemp.SHIPTOATTN2POS = ObjTemp.SHIPTOATTN2POS;
                    //ObjATemp.SHIPTOTERMS = ObjTemp.SHIPTOTERMS;
                    ObjATemp.VENDORADDR = ObjTemp.VENDORADDR;
                    ObjATemp.VENDORATTN1FAX = ObjTemp.VENDORATTN1FAX;
                    ObjATemp.VENDORATTN1MOB = ObjTemp.VENDORATTN1MOB;
                    ObjATemp.VENDORATTN1NAME = ObjTemp.VENDORATTN1NAME;
                    ObjATemp.VENDORATTN1POS = ObjTemp.VENDORATTN1POS;
                    ObjATemp.VENDORATTN1TEL = ObjTemp.VENDORATTN1TEL;
                    ObjATemp.VENDORATTN2FAX = ObjTemp.VENDORATTN2FAX;
                    ObjATemp.VENDORATTN2MOB = ObjTemp.VENDORATTN2MOB;
                    ObjATemp.VENDORATTN2NAME = ObjTemp.VENDORATTN2NAME;
                    ObjATemp.VENDORATTN2POS = ObjTemp.VENDORATTN2POS;
                    ObjATemp.VENDORATTN2TEL = ObjTemp.VENDORATTN2TEL;
                    ObjATemp.VENDORID = ObjTemp.VENDORID;
                    ObjATemp.VENDORNAME = ObjTemp.VENDORNAME;

                    db.A_POTEMPLATEs.InsertOnSubmit(ObjATemp);
                    db.SubmitChanges();
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
    #endregion

    #region A_POLINE
    public partial class A_POLINE
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string SaveRecordInAuditPOLines(long POLINEID, string UserName, string Action)
        {
            try
            {
                //POLines
                POLINE ObjPOLine = db.POLINEs.SingleOrDefault(x => x.POLINEID == POLINEID);
                if (ObjPOLine != null)
                {
                    A_POLINE ObjAPOLine = new A_POLINE();

                    ObjAPOLine.AUDITACTION = Action;
                    ObjAPOLine.AUDITBY = UserName;
                    ObjAPOLine.AUDITTIMESTAMP = DateTime.Now;

                    ObjAPOLine.POLINEID = ObjPOLine.POLINEID;
                    ObjAPOLine.PONUM = ObjPOLine.PONUM;
                    ObjAPOLine.POREVISION = ObjPOLine.POREVISION;
                    ObjAPOLine.COSTCODE = ObjPOLine.COSTCODE;
                    ObjAPOLine.ORDERQTY = ObjPOLine.ORDERQTY;
                    ObjAPOLine.ORDERUNIT = ObjPOLine.ORDERUNIT;
                    ObjAPOLine.DESCRIPTION = ObjPOLine.DESCRIPTION;
                    ObjAPOLine.UNITCOST = ObjPOLine.UNITCOST;
                    ObjAPOLine.LINECOST = ObjPOLine.LINECOST; 
                    ObjAPOLine.LINETYPE = ObjPOLine.LINETYPE; 

                    db.A_POLINEs.InsertOnSubmit(ObjAPOLine);
                    db.SubmitChanges();
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public int getMaxPoLine(int PoNum, int PoRevision)
        {
            int PoLineID = 0;
             SqlConnection rConn = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ToString());
            if (rConn.State == ConnectionState.Open)
            {
                rConn.Close();
            }
            SqlCommand SqlCmd = new SqlCommand("Select MAX(POLINENUM) from POLINE where PONUM="+PoNum+" and POREVISION = "+PoRevision, rConn);          
            SqlCmd.CommandType = CommandType.Text;
            rConn.Open();
            var a = SqlCmd.ExecuteScalar().ToString();
            if (a != null)
            {
                PoLineID = int.Parse(SqlCmd.ExecuteScalar().ToString());
            }   
            return PoLineID;
        }
    }
    #endregion 

    #region A_PO
    public partial class A_PO
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string SaveRecordInAuditPO(int PONum, string UserName, string Action)
        {
            try
            {
                PO ObjPO = db.POs.SingleOrDefault(x => x.PONUM == PONum);
                if (ObjPO != null)
                {
                    A_PO ObjAPO = new A_PO();
                    if (ObjAPO != null)
                    {
                        ObjAPO.AUDITACTION = Action;
                        ObjAPO.AUDITBY = UserName;
                        ObjAPO.AUDITTIMESTAMP = DateTime.Now;

                        ObjAPO.PONUM = ObjPO.PONUM;
                        ObjAPO.DESCRIPTION = ObjPO.DESCRIPTION;
                        ObjAPO.ORGCODE = ObjPO.ORGCODE;
                        ObjAPO.PROJECTCODE = ObjPO.PROJECTCODE;
                        ObjAPO.MRNUM = ObjPO.MRNUM;
                        ObjAPO.QNUM = ObjPO.QNUM;
                        ObjAPO.QDATE = ObjPO.QDATE;
                        ObjAPO.PAYMENTTERMS = ObjPO.PAYMENTTERMS;
                        ObjAPO.ORDERDATE = ObjPO.ORDERDATE;
                        ObjAPO.REQUIREDATE = ObjPO.REQUIREDATE;
                        ObjAPO.VENDORDATE = ObjPO.VENDORDATE;
                        ObjAPO.POTYPE = ObjPO.POTYPE;
                        ObjAPO.ORIGINALPONUM = ObjPO.ORIGINALPONUM;
                        ObjAPO.STATUS = ObjPO.STATUS;
                        ObjAPO.STATUSDATE = ObjPO.STATUSDATE;
                        ObjAPO.BUYERCODE = ObjPO.BUYERCODE;
                        ObjAPO.BUYERNAME = ObjPO.BUYERNAME;
                        ObjAPO.VENDORID = ObjPO.VENDORID;
                        ObjAPO.VENDORNAME = ObjPO.VENDORNAME;
                        ObjAPO.VENDORADDR = ObjPO.VENDORADDR;
                        ObjAPO.VENDORATTN1NAME = ObjPO.VENDORATTN1NAME;
                        ObjAPO.VENDORATTN1TEL = ObjPO.VENDORATTN1TEL;
                        ObjAPO.VENDORATTN1MOB = ObjPO.VENDORATTN1MOB;
                        ObjAPO.VENDORATTN1FAX = ObjPO.VENDORATTN1FAX;
                        ObjAPO.VENDORATTN1POS = ObjPO.VENDORATTN1POS;
                        ObjAPO.VENDORATTN2NAME = ObjPO.VENDORATTN2NAME;
                        ObjAPO.VENDORATTN2TEL = ObjPO.VENDORATTN2TEL;
                        ObjAPO.VENDORATTN2MOB = ObjPO.VENDORATTN2MOB;
                        ObjAPO.VENDORATTN2FAX = ObjPO.VENDORATTN2FAX;
                        ObjAPO.VENDORATTN2POS = ObjPO.VENDORATTN2POS;
                        ObjAPO.SHIPTOADDR = ObjPO.SHIPTOADDR;
                        //ObjAPO.SHIPTOTERMS = ObjPO.SHIPTOTERMS;
                        ObjAPO.SHIPTOATTN1NAME = ObjPO.SHIPTOATTN1NAME;
                        ObjAPO.SHIPTOATTN1MOB = ObjPO.SHIPTOATTN1MOB;
                        ObjAPO.SHIPTOATTN1POS = ObjPO.SHIPTOATTN1POS;
                        ObjAPO.SHIPTOATTN2NAME = ObjPO.SHIPTOATTN2NAME;
                        ObjAPO.SHIPTOATTN2MOB = ObjPO.SHIPTOATTN2MOB;
                        ObjAPO.SHIPTOATTN2POS = ObjPO.SHIPTOATTN2POS; 
                        ObjAPO.CONTRACTREFNUM = ObjPO.CONTRACTREFNUM;
                        //ObjAPO.ROWSTAMP = ObjPO.ROWSTAMP;
                        db.A_POs.InsertOnSubmit(ObjAPO);
                        db.SubmitChanges();
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
    #endregion

    #region Project
    public partial class Project
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string GetProjectName(string ProjectCode)
        {
            try
            {
                string SupID = string.Empty; 
                Project Sup = db.Projects.SingleOrDefault(x => x.ProjectCode == ProjectCode);
                if (Sup == null)
                {
                    SupID = "Project not found";
                }
                else
                {
                    SupID = Sup.ProjectDesc;
                }
                return SupID;
            }
            catch (Exception ex)
            {
                return false.ToString();
            }
        }
        public string GetProjectID(string ProjectName)
        {
            try
            {
                string SupID = string.Empty;
                Project Sup = db.Projects.SingleOrDefault(x => x.ProjectDesc == ProjectName);
                if (Sup == null)
                {
                    SupID = "Project not found";
                }
                else
                {
                    SupID = Sup.ProjectCode;
                }
                return SupID;
            }
            catch (Exception ex)
            {
                return false.ToString();
            }
        }
        public string ValidateProjectName(string ProjectName, string orgID)
        {
            string orgCode = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("FIRMS_VerifyProjects", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure; 

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        { 
                            cmd.Parameters.AddWithValue("@INPUTORGCODE", int.Parse(orgID.Trim()));
                            cmd.Parameters.AddWithValue("@PROJECTNAME", ProjectName.Trim());
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count>0)
                            {
                                orgCode = dt.Rows[0]["depm_code"].ToString();
                            }
                        } 
                        con.Close();                       
                    }
                } 

                return orgCode;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ValidateUsingProjectCode(string ProjectCode, string orgID)
        {
            string orgName = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("FIRMS_VerifyProjectsUsingProjectCode", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            cmd.Parameters.AddWithValue("@INPUTORGCODE", int.Parse(orgID.Trim()));
                            cmd.Parameters.AddWithValue("@PROJECTCode", ProjectCode.Trim());
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                orgName = dt.Rows[0]["depm_desc"].ToString() +";;" + dt.Rows[0]["depm_code"].ToString();
                            }
                        }
                        con.Close();
                    }
                }

                return orgName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ValidateOrganization(string OrgName)
        {
            string orgCode = string.Empty;
            try
            {
                string value = string.Empty;
                var masg = db.FIRMS_VerifyOrgs(OrgName);
                foreach (var m in masg)
                {
                    orgCode = m.org_code.ToString() + ";" +m.org_name.ToString();
                }
                return orgCode;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ValidateBuyerUserName(string UserName)
        {
            string UserID = string.Empty;
            try
            {
                string value = string.Empty;
                var masg = from Usr in db.Users
                           join
                           Sec in db.SS_UserSecurityGroups on Usr.UserID equals Sec.UserID
                           where Sec.SecurityGroupID == 3 && Usr.Status =="ACT" && (Usr.UserID == UserName || Usr.FirstName == UserName)
                           select new { Usr.UserID, Usr.FirstName, Usr.LastName};

                foreach (var m in masg)
                {
                    UserID = m.UserID.ToString();
                }
                return UserID;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ValidateBuyerUserID(int ID)
        {
            string Emp_Name = string.Empty;
            try
            {
                string value = string.Empty;
                var masg = db.FIRMS_ValidateEmployeeByID(ID);

                foreach (var m in masg)
                {
                    Emp_Name = m.emp_name;
                }
                return Emp_Name;
            }
            catch (Exception ex)
            {
                return "Exception: "+ex.Message;
            }
        }
        public string getFirstName(string UserName)
        {
            string UserID = string.Empty;
            try
            {
                string value = string.Empty;
                var masg = from Usr in db.Users
                           join
                           Sec in db.SS_UserSecurityGroups on Usr.UserID equals Sec.UserID
                           where Sec.SecurityGroupID == 3 && Usr.Status == "ACT" && (Usr.UserID == UserName || Usr.FirstName == UserName)
                           select new { Usr.UserID, Usr.FirstName, Usr.LastName };

                foreach (var m in masg)
                {
                    UserID = m.FirstName.ToString();
                }
                return UserID;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ValidatePurchaseType(string Value)
        {
            string UserID = string.Empty;
            try
            {
                string value = string.Empty;
                var masg = from Sec in db.SS_ALNDomains
                           where Sec.DomainName == "POTYPE" && Sec.Value == Value
                           select new { Sec.Value, Sec.Description};

                foreach (var m in masg)
                {
                    UserID = m.Description.ToString();
                }
                return UserID;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ValidateFromDomainTableValue(string DomainName, string Value)
        {
            string ValueID = string.Empty;
            try
            {
                string value = string.Empty;
                var masg = from Sec in db.SS_ALNDomains
                           where Sec.DomainName == DomainName && Sec.Value == Value
                           select new { Sec.Value, Sec.Description };

                foreach (var m in masg)
                {
                    ValueID = m.Value.ToString();
                }
                return ValueID;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ValidateFromDomainTableDescription(string DomainName, string Value)
        {
            string ValueID = string.Empty;
            try
            {
                string value = string.Empty;
                var masg = from Sec in db.SS_ALNDomains
                           where Sec.DomainName == DomainName && Sec.Value == Value
                           select new { Sec.Value, Sec.Description };

                foreach (var m in masg)
                {
                    ValueID = m.Value.ToString();
                }
                return ValueID;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string GetPurchaseType(string Value)
        {
            string UserID = string.Empty;
            try
            {
                string value = string.Empty;
                var masg = from Sec in db.SS_ALNDomains
                           where Sec.DomainName == "POTYPE" && Sec.Value == Value
                           select new { Sec.Value, Sec.Description };

                foreach (var m in masg)
                {
                    UserID = m.Description.ToString();
                }
                return UserID;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ValidateSupplierID(int SupplierID)
        {
            string TempSupplierID = string.Empty;
            try
            {
                string value = string.Empty;
                var masg = from Sec in db.Suppliers
                           where Sec.SupplierID == SupplierID
                           select new {Sec.SupplierName};

                foreach (var m in masg)
                {
                    TempSupplierID = m.SupplierName.ToString();
                }
                return TempSupplierID;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ValidateContractName(string Contractname)
        {
            string ContractName = string.Empty;
            try
            {
                string value = string.Empty;
                var masg = from Sec in db.SS_ALNDomains
                           where Sec.DomainName == "CONTRACTTYPE" && Sec.Description == Contractname
                           select new { Sec.Value, Sec.Description };

                foreach (var m in masg)
                {
                    ContractName = m.Value.ToString();
                }
                return ContractName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string getContractTypeName(string ContractType, string CreatedBy)
        {
            string ContractName = string.Empty;
            try
            {
                string value = string.Empty;
                var masg = from Sec in db.SS_ALNDomains
                           where Sec.DomainName == "CONTRACTTYPE" && Sec.Value == ContractType
                           select new { Sec.Value, Sec.Description };

                foreach (var m in masg)
                {
                    ContractName = m.Description.ToString();
                }
                return ContractName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string VerifyContractID(int ContractID)
        {
            string ContractName = string.Empty;
            try
            {
                string value = string.Empty;
                var masg = from Sec in db.CONTRACTs
                           where Sec.CONTRACTNUM == ContractID
                           select new { Sec.CONTRACTNUM };

                foreach (var m in masg)
                {
                    ContractName = m.CONTRACTNUM.ToString();
                }
                return ContractName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ValidateContractNameUsingValue(string ContractValue)
        {
            string ContractName = string.Empty;
            try
            {
                string value = string.Empty;
                var masg = from Sec in db.SS_ALNDomains
                           where Sec.DomainName == "CONTRACTTYPE" && Sec.Value == ContractValue
                           select new { Sec.Value, Sec.Description };

                foreach (var m in masg)
                {
                    ContractName = m.Description.ToString();
                }
                return ContractName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
    #endregion

    #region SS_ALNDomain
    public partial class SS_ALNDomain
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());

        public string GetOrganizationCode(string OrgCode)
        {
            try
            {
                string SupID = string.Empty;
                SS_ALNDomain Sup = db.SS_ALNDomains.SingleOrDefault(x => x.Value == OrgCode);
                if (Sup == null)
                {
                    SupID = "Organization not found";
                }
                else
                {
                    SupID = Sup.Description;
                }
                return SupID;
            }
            catch (Exception ex)
            {
                return false.ToString();
            }
        }

        public string GetStatusCode(string Status,string DomainName)
        {
            try
            {
                string SupID = string.Empty;
                SS_ALNDomain Sup = db.SS_ALNDomains.SingleOrDefault(x => x.Description == Status && x.DomainName == DomainName);
                if (Sup == null)
                {
                    SupID = "Status Code not found";
                }
                else
                {
                    SupID = Sup.Value;
                }
                return SupID;
            }
            catch (Exception ex)
            {
                return false.ToString();
            }
        }
    }
#endregion


    #region SS_NumDomain
    public partial class SS_NumDomain
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());

        public string GetIsVatRegisteredValue(string Value)
        {
            try
            {
                string SupID = string.Empty;
                SS_NumDomain Sup = db.SS_NumDomains.SingleOrDefault(x => x.Value == int.Parse(Value));
                if (Sup != null)
                {
                    if (Sup.Value == 1)
                    {
                        SupID = true.ToString();
                    }
                    if (Sup.Value == 0)
                    {
                        SupID = false.ToString();
                    }
                }
                return SupID;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string GetIsVatRegisteredValueDescription(string Value)
        {
            string SupID = string.Empty;
            try
            {
                if (Value == "true" || Value == "True")
                {
                    SupID = "1";
                }
                if (Value == "false" || Value == "False")
                {
                    SupID = "0";
                }

                return SupID;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
    #endregion

    #region A_ProjectMember
    public partial class A_ProjectMember
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string SaveRecordInAuditProjectMember(string userID, string Action, string orgCode, string ProjectCode, int ProjectRoleID, string EmpCode)
        {
            string masg = string.Empty;
            try
            {
                ProjectMember ObjTeam = db.ProjectMembers.FirstOrDefault(x => x.ORGCODE == orgCode && x.PROJECTCODE == ProjectCode && x.PROJECTROLEID == ProjectRoleID && x.TEAMMEMBERCODE == EmpCode);

                if (ObjTeam != null)
                {
                    A_ProjectMember ObjATeam = new A_ProjectMember();
                    ObjATeam.AuditAction = Action;
                    ObjATeam.AuditBy = userID;
                    ObjATeam.AuditTimeStamp = DateTime.Now; 
                    ObjATeam.ORGCODE = orgCode;
                    ObjATeam.ORGNAME = ObjTeam.ORGNAME;
                    ObjATeam.PROJECTROLEID = ObjTeam.PROJECTROLEID;
                    ObjATeam.PROJECTCODE = ProjectCode;
                    ObjATeam.PROJECTNAME = ObjTeam.PROJECTNAME;
                    ObjATeam.TEAMMEMBERCODE = EmpCode;
                    ObjATeam.TEAMMEMBERNAME = ObjTeam.TEAMMEMBERNAME;
                    db.A_ProjectMembers.InsertOnSubmit(ObjATeam);
                    db.SubmitChanges();
                    masg =  "Success";
                }

            }
            catch (Exception ex)
            {
                masg =  ex.Message;
            }
            return masg;
        } 
    }
    #endregion

    #region A_POSignatureTemplate
    public partial class A_POSignatureTemplate
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string SaveRecordInAuditSignature(string userID, string Action, int POSignID)
        {
            string masg = string.Empty;
            try
            {
                POSignatureTemplate ObjTeam = db.POSignatureTemplates.FirstOrDefault(x => x.POSignID == POSignID);

                if (ObjTeam != null)
                {
                    A_POSignatureTemplate ObjATeam = new A_POSignatureTemplate();
                    ObjATeam.AuditAction = Action;
                    ObjATeam.AuditBy = userID;
                    ObjATeam.AuditTimeStamp = DateTime.Now;
                    ObjATeam.CreatedBy = userID;
                    ObjATeam.CreationDateTime = DateTime.Now;
                    ObjATeam.OrgCode = ObjTeam.OrgCode;
                    ObjATeam.OrgName = ObjTeam.OrgName;
                    ObjATeam.POSignID = ObjTeam.POSignID;
                    ObjATeam.TeamMemberCode = ObjTeam.TeamMemberCode;
                    ObjATeam.TeamMemberName = ObjTeam.TeamMemberName;
                    ObjATeam.Title = ObjTeam.Title;
                    ObjATeam.OrderNo = ObjTeam.OrderNo;
                    ObjATeam.Heading = ObjTeam.Heading; 
                    db.A_POSignatureTemplates.InsertOnSubmit(ObjATeam);
                    db.SubmitChanges();
                    masg = "Success";
                }

            }
            catch (Exception ex)
            {
                masg = ex.Message;
            }
            return masg;
        }
    }
    #endregion

    #region ContractStatusHistory
    public partial class ContractStatusHistory
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        public string ChangeContractStatus(int ContractNum, string OLDStatus, string NewStatus, string Memo, string userName)
        {
            string masg = string.Empty;
            try
            {
            //    CONTRACT ObjContract = db.CONTRACTs.SingleOrDefault(x => x.CONTRACTNUM == ContractNum);
            //    if (ObjContract != null)
            //    {
            //        ObjContract.STATUS = NewStatus;
            //        ObjContract.STATUSDATE = DateTime.Now;
            //        ObjContract.LASTMODIFIEDBY = userName;
            //        ObjContract.LASTMODIFIEDDATE = DateTime.Now;
            //        //db.SubmitChanges();

                    ContractStatusHistory ObjConStatus = new ContractStatusHistory();
                    ObjConStatus.CONTRACTNUM = ContractNum;
                    ObjConStatus.OLDSTATUS = OLDStatus;
                    ObjConStatus.NEWSTATUS = NewStatus;
                    ObjConStatus.MEMO = Memo;
                    ObjConStatus.MODIFIEDBY = userName;
                    ObjConStatus.MODIFICATIONDATE = DateTime.Now;
                    db.ContractStatusHistories.InsertOnSubmit(ObjConStatus);
                    db.SubmitChanges();
                    return "Success";
                //}
            }
            catch (Exception ex)
            {
                masg = ex.Message;
            }
            return masg;
        }
    }
    #endregion
}