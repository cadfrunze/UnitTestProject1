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


        [TestMethod]
        public void TestMethod_Sigemo()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(SIGEMO_TARGET);
            var wait = driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20); // Waiting.... sa ma asigur ca incarca acel popup

            try
            {
                var buton_close = driver.FindElement(By.XPath("//button")); // Acel popup futa-l draqu'
                buton_close.Click();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            IWebElement box_search = driver.FindElement(By.XPath("//search-input.tt-input"));
            box_search.Clear();
            box_search.SendKeys(BRAND_NAME);


            //var menuOpen = driver.FindElement(By.PartialLinkText("Logare"));
            //menuOpen.Click();
            //driver.Quit();
            
        }
    }
}
