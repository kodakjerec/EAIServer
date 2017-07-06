using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AgentAIServer.MyService
{
    class eMailFunctions
    {
        /// <summary>
        /// 寄信標題
        /// </summary>
        string mailTitle = "";
        /// <summary>
        /// 寄信人Email
        /// </summary>
        string sendMail = "";
        /// <summary>
        /// 收信人Email(多筆用逗號隔開)
        /// </summary>
        string receiveMails = "";

        /// <summary>
        /// 副本Email(多筆用逗號隔開)
        /// </summary>
        string CcsMails = "";

        /// <summary>
        /// 寄信smtp server
        /// </summary>
        string smtpServer = "";

        /// <summary>
        /// 寄信smtp server的Port，預設25
        /// </summary>
        int smtpPort = 25;

        /// <summary>
        /// 寄信帳號
        /// </summary>
        string mailAccount = "";

        /// <summary>
        /// 寄信密碼
        /// </summary>
        string mailPwd = "";

        #region Mail
        /// <summary>
        /// 讀取寄信設定
        /// </summary>
        public void eMailSettings_Get()
        {
            StreamReader sr = new StreamReader("JSON\\eMail_Settings.txt", Encoding.GetEncoding("utf-8"));
            try
            {
                DataTable eMailSettings = DB_IO.JSONconvert.JSONstrToDataTable(sr.ReadToEnd());

                /// <summary>
                /// 寄信人Email
                /// </summary>
                sendMail = eMailSettings.Rows[0]["sendMail"].ToString();
                /// <summary>
                /// 收信人Email(多筆用逗號隔開)
                /// </summary>
                receiveMails = eMailSettings.Rows[0]["receiveMails"].ToString();

                /// <summary>
                /// 副本Email(多筆用逗號隔開)
                /// </summary>
                CcsMails = eMailSettings.Rows[0]["CcsMails"].ToString();

                /// <summary>
                /// 寄信smtp server
                /// </summary>
                smtpServer = eMailSettings.Rows[0]["smtpServer"].ToString();

                /// <summary>
                /// 寄信smtp server的Port，預設25
                /// </summary>
                smtpPort = Convert.ToInt32(eMailSettings.Rows[0]["smtpPort"]);

                /// <summary>
                /// 寄信帳號
                /// </summary>
                mailAccount = eMailSettings.Rows[0]["mailAccount"].ToString();

                /// <summary>
                /// 寄信密碼
                /// </summary>
                mailPwd = eMailSettings.Rows[0]["mailPwd"].ToString();
            }
            catch
            {

            }
            finally
            {
                sr.Close();
            }
        }

        /// <summary>
        /// 完整的寄信函數
        /// </summary>
        /// <param name="MailFrom">寄信人Email Address</param>
        /// <param name="MailTos">收信人Email Address</param>
        /// <param name="Ccs">副本Email Address</param>
        /// <param name="MailSub">主旨</param>
        /// <param name="MailBody">內文</param>
        /// <param name="isBodyHtml">是否為Html格式</param>
        /// <param name="files">要夾帶的附檔</param>
        /// <returns>回傳寄信是否成功(true:成功,false:失敗)</returns>
        public bool Mail_Send(string MailFrom, string[] MailTos, string[] Ccs, string MailSub, string MailBody, bool isBodyHtml, Dictionary<string, Stream> files)
        {
            try
            {
                //沒給寄信人mail address
                if (string.IsNullOrEmpty(MailFrom))
                {//※有些公司的Smtp Server會規定寄信人的Domain Name須是該Smtp Server的Domain Name，例如底下的 system.com.tw
                    return false;
                }

                //命名空間： System.Web.Mail已過時，http://msdn.microsoft.com/zh-tw/library/system.web.mail.mailmessage(v=vs.80).aspx
                //建立MailMessage物件
                MailMessage mms = new MailMessage();
                //指定一位寄信人MailAddress
                mms.From = new MailAddress(MailFrom);
                //信件主旨
                mms.Subject = MailSub;
                //信件內容
                mms.Body = MailBody;
                //信件內容 是否採用Html格式
                mms.IsBodyHtml = isBodyHtml;

                if (MailTos != null)//防呆
                {
                    for (int i = 0; i < MailTos.Length; i++)
                    {
                        //加入信件的收信人(們)address
                        if (!string.IsNullOrEmpty(MailTos[i].Trim()))
                        {
                            mms.To.Add(new MailAddress(MailTos[i].Trim()));
                        }

                    }
                }//End if (MailTos !=null)//防呆

                if (Ccs != null) //防呆
                {
                    for (int i = 0; i < Ccs.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(Ccs[i].Trim()))
                        {
                            //加入信件的副本(們)address
                            mms.CC.Add(new MailAddress(Ccs[i].Trim()));
                        }

                    }
                }//End if (Ccs!=null) //防呆


                //附件處理
                if (files != null && files.Count > 0)//寄信時有夾帶附檔
                {
                    foreach (string fileName in files.Keys)
                    {
                        Attachment attfile = new Attachment(files[fileName], fileName);
                        mms.Attachments.Add(attfile);
                    }//end foreach
                }//end if 

                using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))//或公司、客戶的smtp_server
                {
                    if (!string.IsNullOrEmpty(mailAccount) && !string.IsNullOrEmpty(mailPwd))//.config有帳密的話
                    {//寄信要不要帳密？眾說紛紜Orz，分享一下經驗談....

                        //網友阿尼尼:http://www.dotblogs.com.tw/kkc123/archive/2012/06/26/73076.aspx
                        //※公司內部不用認證,寄到外部信箱要特別認證 Account & Password

                        //自家公司MIS:
                        //※要看smtp server的設定呀~

                        //結論...
                        //※程式在客戶那邊執行的話，問客戶，程式在自家公司執行的話，問自家公司MIS，最準確XD
                        client.Credentials = new NetworkCredential(mailAccount, mailPwd);//寄信帳密
                    }
                    client.Send(mms);//寄出一封信
                }//end using 
                 //釋放每個附件，才不會Lock住
                if (mms.Attachments != null && mms.Attachments.Count > 0)
                {
                    for (int i = 0; i < mms.Attachments.Count; i++)
                    {
                        mms.Attachments[i].Dispose();

                    }
                }

                return true;//成功
            }
            catch (Exception ex)
            {
                //寄信失敗，寫Log文字檔
                //recLog("寄信失敗 " + ex, "Mail");
                return false;
            }
        }//End 寄信
        public bool Mail_Send(string MailSub, string MailBody)
        {
            try
            {
                //沒給寄信人mail address
                if (string.IsNullOrEmpty(sendMail))
                {//※有些公司的Smtp Server會規定寄信人的Domain Name須是該Smtp Server的Domain Name，例如底下的 system.com.tw
                    return false;
                }

                //命名空間： System.Web.Mail已過時，http://msdn.microsoft.com/zh-tw/library/system.web.mail.mailmessage(v=vs.80).aspx
                //建立MailMessage物件
                MailMessage mms = new MailMessage();
                //指定一位寄信人MailAddress
                mms.From = new MailAddress(sendMail);
                //信件主旨
                mms.Subject = MailSub;
                //信件內容
                mms.Body = MailBody;
                //信件內容 是否採用Html格式
                mms.IsBodyHtml = true;

                if (receiveMails != null)//防呆
                {
                    string[] MailTos = receiveMails.Split(',');

                    for (int i = 0; i < MailTos.Length; i++)
                    {
                        //加入信件的收信人(們)address
                        if (!string.IsNullOrEmpty(MailTos[i].Trim()))
                        {
                            mms.To.Add(new MailAddress(MailTos[i].Trim()));
                        }

                    }
                }//End if (MailTos !=null)//防呆

                if (CcsMails != null) //防呆
                {
                    string[] Ccs = CcsMails.Split(',');

                    for (int i = 0; i < Ccs.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(Ccs[i].Trim()))
                        {
                            //加入信件的副本(們)address
                            mms.CC.Add(new MailAddress(Ccs[i].Trim()));
                        }

                    }
                }//End if (Ccs!=null) //防呆

                using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))//或公司、客戶的smtp_server
                {
                    if (!string.IsNullOrEmpty(mailAccount) && !string.IsNullOrEmpty(mailPwd))//.config有帳密的話
                    {//寄信要不要帳密？眾說紛紜Orz，分享一下經驗談....

                        //網友阿尼尼:http://www.dotblogs.com.tw/kkc123/archive/2012/06/26/73076.aspx
                        //※公司內部不用認證,寄到外部信箱要特別認證 Account & Password

                        //自家公司MIS:
                        //※要看smtp server的設定呀~

                        //結論...
                        //※程式在客戶那邊執行的話，問客戶，程式在自家公司執行的話，問自家公司MIS，最準確XD
                        client.Credentials = new NetworkCredential(mailAccount, mailPwd);//寄信帳密
                    }
                    client.Send(mms);//寄出一封信
                }//end using 
                 //釋放每個附件，才不會Lock住
                if (mms.Attachments != null && mms.Attachments.Count > 0)
                {
                    for (int i = 0; i < mms.Attachments.Count; i++)
                    {
                        mms.Attachments[i].Dispose();

                    }
                }

                return true;//成功
            }
            catch(Exception ex)
            {
                //寄信失敗，寫Log文字檔
                Program.recLog("寄信失敗 " + ex.Message+ ex.InnerException.ToString(), "Mail");
                return false;
            }
        }//End 寄信
        #endregion
    }
}
