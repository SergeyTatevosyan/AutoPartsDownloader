using System;
using System.Collections.Generic;
using AE.Net.Mail;
using Limilabs.Client.IMAP;
using Limilabs.Mail;
using Limilabs.Mail.MIME;

namespace AutoPartsDownloader.Services
{
    public static class MailParser
    {
        public static void DownloadAttachmentFromMail()
        {
            try
            {
                using (Imap imap = new Imap())
                {
                    imap.Connect("imap.gmail.com",993);
                    imap.UseBestLogin("login@gmail.com", "password");

                    imap.SelectInbox();
                    List<long> uids = imap.Search(Flag.All);

                    foreach (long uid in uids)
                    {
                        IMail email = new MailBuilder()
                            .CreateFromEml(imap.GetMessageByUID(uid));
                        if (ColumnConfiguration.ProviderColumnName.ContainsKey(email.From.ToString()) &&
                            email.Date == DateTime.Now.Date)
                        {//Если есть поставщик в списке наименований колонок
                            Console.WriteLine(email.Subject);

                            foreach (MimeData mime in email.Attachments)
                            {
                                mime.Save(mime.SafeFileName);
                            }
                        }
                    }
                    imap.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
