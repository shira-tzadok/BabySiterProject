using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
//using System.Net.Mail;
using System.Web;

namespace babySiter.Models
{
    public class SendEmail
    {

        private static BabySitterDBEntities db = new BabySitterDBEntities();
        private static MailMessage mail = new MailMessage();

        //Babysitter
        public static void send(int IdMother, int IdBS, string bodyEmail, int IdInvaite, MailAddress address, string subject)
        {
            Mother mother = db.Mother.FirstOrDefault(x => x.Id == IdMother);//ToDo האם צריך פה את שתיהם????
            BabySitter babySitter = db.BabySitter.FirstOrDefault(x => x.Id == IdBS);
            Invaite invaite = db.Invaite.FirstOrDefault(x => x.Id == IdInvaite);
            PartOfDay partOfDay = db.PartOfDay.FirstOrDefault(x => x.IdPart == invaite.IdPart);

            mail.To.Add(address);
            mail.From = new MailAddress("servicebabysittersite@gmail.com");
            mail.Subject = subject;
            mail.Body = "<body dir='ltr' style='margin: 30px'><center>" +
                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #ea8d85;'> Babysitter Request </label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> By :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + mother.LastName + " " + mother.FirstName + "</label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> On the date :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + invaite.Date + "</label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> Part of the day :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + partOfDay.Part + "</label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> More :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + bodyEmail + "</label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;'> To confirm:</label><a href='http://localhost:4200/confirmation/" + invaite.Id + "'>Click here</a>" +
                "</center></body> ";

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential(
                "servicebabysittersite@gmail.com", "bsm12345");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                /////  Console.WriteLine(ex);             
            }
        }

        public static void sendCancel(int IdMother, int IdBS, string bodyEmail, int IdInvaite, MailAddress address, string subject)
        {
            Mother mother = db.Mother.FirstOrDefault(x => x.Id == IdMother);//ToDo האם צריך פה את שתיהם????
            BabySitter babySitter = db.BabySitter.FirstOrDefault(x => x.Id == IdBS);
            Invaite invaite = db.Invaite.FirstOrDefault(x => x.Id == IdInvaite);
            PartOfDay partOfDay = db.PartOfDay.FirstOrDefault(x => x.IdPart == invaite.IdPart);

            mail.To.Add(address);
            mail.From = new MailAddress("servicebabysittersite@gmail.com");
            mail.Subject = subject;
            //mail.Body = bodyEmail + "<br> <label style='color:red'>To confirm the order:</label><a href='http://localhost:4200/confirmation/" + IdInvite + "'>Click here</a>";
            mail.Body = "<body dir='ltr' style='margin: 30px'><center>" +
                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #ea8d85;'> Cancel Reservation </label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> By :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + mother.LastName + " " + mother.FirstName + "</label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> On the date :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + invaite.Date + "</label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> Part of the day :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + partOfDay.Part + "</label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> More :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + bodyEmail + "</label><br >" +

                "</center></body> ";

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential(
                "servicebabysittersite@gmail.com", "bsm12345");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                /////  Console.WriteLine(ex);             
            }
        }




        //Mother
        public static void sendMother(int IdMother, int IdBS, string bodyEmail, int IdInvaite, MailAddress address, string subject)
        {
            Mother mother = db.Mother.FirstOrDefault(x => x.Id == IdMother);//ToDo האם צריך פה את שתיהם????
            BabySitter babySitter = db.BabySitter.FirstOrDefault(x => x.Id == IdBS);
            Invaite invaite = db.Invaite.FirstOrDefault(x => x.Id == IdInvaite);
            PartOfDay partOfDay = db.PartOfDay.FirstOrDefault(x => x.IdPart == invaite.IdPart);

            mail.To.Add(address);
            mail.From = new MailAddress("servicebabysittersite@gmail.com");
            mail.Subject = subject;
            mail.Body = "<body dir='ltr' style='margin: 30px'><center>" +
                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #ea8d85;'> Apply For </label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> By :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + babySitter.LastName + " " + babySitter.FirstName + "</label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> On the date :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + invaite.Date + "</label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> Part of the day :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + partOfDay.Part + "</label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> More :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + bodyEmail + "</label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;'> To Confirm:</label><a href='http://localhost:4200/confirmation/" + invaite.Id + "'>Click here</a>" +
                "</center></body> ";

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential(
                "servicebabysittersite@gmail.com", "bsm12345");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                /////  Console.WriteLine(ex);             
            }
        }
        public static void sendCancelMother(int IdMother, int IdBS, string bodyEmail, int IdInvaite, MailAddress address, string subject)
        {
            Mother mother = db.Mother.FirstOrDefault(x => x.Id == IdMother);//ToDo האם צריך פה את שתיהם????
            BabySitter babySitter = db.BabySitter.FirstOrDefault(x => x.Id == IdBS);
            Invaite invaite = db.Invaite.FirstOrDefault(x => x.Id == IdInvaite);
            PartOfDay partOfDay = db.PartOfDay.FirstOrDefault(x => x.IdPart == invaite.IdPart);

            mail.To.Add(address);
            mail.From = new MailAddress("servicebabysittersite@gmail.com");
            mail.Subject = subject;
            mail.Body = "<body dir='ltr' style='margin: 30px'><center>" +
                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #ea8d85;'> Cancel Reservation </label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> By :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + babySitter.LastName + " " + babySitter.FirstName + "</label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> On the date :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + invaite.Date + "</label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> Part of the day :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + partOfDay.Part + "</label><br >" +

                "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> More :</label>" +
                "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + bodyEmail + "</label><br >" +

                "</center></body> ";

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential(
                "servicebabysittersite@gmail.com", "bsm12345");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                /////  Console.WriteLine(ex);             
            }
        }



        public static void sendEmailConfirmAddToGroup(int IdManagement, bool IsMother, int IdGroup, MailAddress address, string subject)
        {
            string nameManagement;
            if (IsMother)
            {
                Mother motherSending = db.Mother.FirstOrDefault(x => x.Id == IdManagement);
                nameManagement = motherSending.FirstName + " " + motherSending.LastName;
            }
            else
            {
                BabySitter babySitterSending = db.BabySitter.FirstOrDefault(x => x.Id == IdManagement);
                nameManagement = babySitterSending.FirstName + " " + babySitterSending.LastName;
            }
            //string nameGroup = db.Group.Where(x => x.GroupId == IdGroup).Select(z => z.NameGroup).ToString();
            Group nameGroup = db.Group.FirstOrDefault(x => x.GroupId == IdGroup);


            mail.To.Add(address);
            mail.From = new MailAddress("servicebabysittersite@gmail.com");
            mail.Subject = subject;
            //mail.Body = "<br> <label>To confirm the order:</label><a href='http://localhost:4200/confirmJoinToGroup/" + IdGroup + "'>Click here</a>";


            mail.Body = "<body dir='ltr' style='margin: 30px'><center>" +
                        "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #ea8d85;'> Request to join the group </label><br >" +

                        "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> To Group :</label>" +
                        "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + nameGroup.NameGroup + "</label><br >" +

                        "<label style = 'font-size: 20px;margin-top:5px;font-family: sans-serif;color: #6d5c7a;'> By Director:</label>" +
                        "<label style = 'font-size: 15px;margin-top:5px;font-family: sans-serif;color: black;'>" + nameManagement + "</label><br >" +

                        "<br> <label>To confirm the order:</label><a href='http://localhost:4200/confirmJoinToGroup/" + IdGroup + "'>Click here</a>"+

                        "</center></body> ";


            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential(
                "servicebabysittersite@gmail.com", "bsm12345");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                /////  Console.WriteLine(ex);             
            }
        }
    }
}