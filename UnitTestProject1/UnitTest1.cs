using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;







namespace UnitTestProject1
{
    
    [TestClass]
    public class UnitTest1
    {   // Adresele URL pt obiectul driver + brandul/marca de anvelope pe care il caut
        readonly string SIGEMO_TARGET = "https://www.sigemo.ro/";
        readonly string NEXXON_TARGET = "https://www.nexxon.ro/";
        readonly string BRAND_NAME = "Hankook";
        readonly string USER_SIGEMO = Environment.GetEnvironmentVariable("user_sigemo");
        bool success = false; //Variabila pt inchidere popup


        [TestMethod]
        public void TestMethod_Sigemo()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(SIGEMO_TARGET);
            var wait = driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20); // Waiting.... sa ma asigur ca incarca acel popup
            

            while (true)
            {
                try
                {
                    var buton_close = driver.FindElement(By.XPath("/html/body/div[7]/div[1]/button")); // Acel popup futa-l draqu'
                    buton_close.Click();
                    success = true;
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                    continue;
                }
                if (success)
                    break;
             
            }
            
            IWebElement box_search = driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/span/input[2]"));
            box_search.Clear();
            box_search.SendKeys(BRAND_NAME);
            
            driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/button")).Click();

            //var menuOpen = driver.FindElement(By.PartialLinkText("Logare"));
            //menuOpen.Click();
            //driver.Quit();

        }
    }
}
