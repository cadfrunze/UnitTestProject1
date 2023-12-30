using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V118.Network;
using System;
using System.Net.NetworkInformation;
using System.Threading;







namespace UnitTestProject1
{
    
    [TestClass]
    public class UnitTest1
    {   // Adresele URL pt obiectul driver + brandul/marca de anvelope pe care il caut
        readonly string SIGEMO_TARGET = "https://www.sigemo.ro/";
        readonly string NEXXON_TARGET = "https://www.nexxon.ro/";
        readonly string BRAND_NAME = "Hankook";
        readonly string USER_SIGEMO = Environment.GetEnvironmentVariable("user_sigemo");
        

        [TestMethod]
        public void TestMethod_Sigemo()
        {
            status_net();
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(SIGEMO_TARGET);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20); // Waiting.... sa ma asigur ca incarca acel popup
            

            while (true) // inchidere popup
            {
                try
                {
                    var buton_close = driver.FindElement(By.XPath("/html/body/div[7]/div[1]/button")); // Acel popup futa-l draqu'
                    buton_close.Click();
                    break;
                }
                catch (Exception) { continue;}
             
            }
            while (true) // inchidere mesaj cookie
            {
                try
                {
                    driver.FindElement(By.XPath("/html/body/div[2]/div/a[1]")).Click();
                    break;
                }
                catch (Exception) { continue; }
            }
            
            // text_box searh
            IWebElement box_search = driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/span/input[2]"));
            box_search.Clear();
            box_search.SendKeys(BRAND_NAME); // adaugare in text box variabila BRAND
            status_net();
            while (true)
            {
                try
                {
                    driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/button"))
                    .Click(); // click icoana "lupa" (search)
                    break;
                }
                catch (Exception) { continue; }
            }
            
            
            driver.FindElement(By.XPath("/html/body/div[7]/div[2]/div/div/div/div/div/div[1]/div/div/div/div[5]/div/div/div/label/select")).Click(); // click box "latime anvelope"
            driver.FindElement(By.XPath("/html/body/div[7]/div[2]/div/div/div/div/div/div[1]/div/div/div/div[5]/div/div/div/label/select/option[10]")).Click(); //selectare dimeniune latime 225
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("/html/body/div[7]/div[2]/div/div/div/div/div/div[1]/div/div/div/div[3]/div/div/div/label/select/option[3]")).Click(); //selectare inaltime 45

            //var menuOpen = driver.FindElement(By.PartialLinkText("Logare"));
            //menuOpen.Click();
            //driver.Quit();

        }

        public void status_net()
            // Verificare conexiune date
        {
            Ping pingul = new Ping();
            while (true)
                try
                {
                    
                    var reply = pingul.Send("www.google.ro");
                    if (reply.Status == IPStatus.Success)
                        break;
                }
                catch (Exception) { continue; }
        }
    }
}
