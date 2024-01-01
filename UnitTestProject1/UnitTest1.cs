using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;









namespace UnitTestProject1
{
    
    [TestClass]
    public class UnitTest1
    {   // Adresele URL pt obiectul driver + brandul/marca de anvelope pe care il caut
        public readonly string SIGEMO_TARGET = "https://www.sigemo.ro/";
        public  string[] ELEM_SEARCH = {" ", "bla bla"};
        public readonly string BRAND_NAME = "Hankook";
        public readonly string USER_SIGEMO = Environment.GetEnvironmentVariable("user_sigemo");
        public IWebDriver driver = new ChromeDriver();

        [TestMethod]
        public void TestMethod_Sigemo()
        {
            status_net(); // Verificare conexiune date
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(SIGEMO_TARGET);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20); // Waiting.... sa ma asigur ca incarca acel popup
            close_popup(); // Inchidere popup
            while (true) // inchidere mesaj cookie
            {
                try
                {
                    driver.FindElement(By.XPath("/html/body/div[2]/div/a[1]")).Click();
                    break;
                }
                catch (Exception)
                {
                    //driver.Navigate().Refresh();
                    continue;
                }
            }
            test_search();
            // text_box search, introducere var BRAND in boxa "search"
            IWebElement box_search = driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/span/input[2]"));
            box_search.Clear();
            box_search.SendKeys(BRAND_NAME); // adaugare in text box variabila BRAND
            status_net();

            /* Acest element din search, dupa introducerea  variabilei BRAND_NAME, functioneaza din "Run", dar din debug,
               imi RETURNEAZA:  
                    OpenQA.Selenium.ElementClickInterceptedException:
                    HResult=0x80131500
                    Message=element click intercepted: Element <button type="button" class="search-button" data-search-url="https://www.sigemo.ro/index.php?route=product/search&amp;search="></button> is not clickable at point (952, 185). Other element would receive the click: <div class="tt-menu .tt-empty tt-open" style="position: absolute; top: 100%; left: 0px; z-index: 100; display: block;">...</div>
                    (Session info: chrome=120.0.6099.130)
                    Source=WebDriver
                    StackTrace:
                    at OpenQA.Selenium.WebDriver.UnpackAndThrowOnError(Response errorResponse, String commandToExecute)
                    at OpenQA.Selenium.WebDriver.Execute(String driverCommandToExecute, Dictionary`2 parameters)
                    at OpenQA.Selenium.WebElement.Execute(String commandToExecute, Dictionary`2 parameters)
                    at UnitTestProject1.UnitTest1.TestMethod_Sigemo() in D:\proiecte C#\UnitTestProject1\UnitTestProject1\UnitTest1.cs:line 60

                    This exception was originally thrown at this call stack:
                    OpenQA.Selenium.WebDriver.UnpackAndThrowOnError(OpenQA.Selenium.Response, string)
                    OpenQA.Selenium.WebDriver.Execute(string, System.Collections.Generic.Dictionary<string, object>)
                    OpenQA.Selenium.WebElement.Execute(string, System.Collections.Generic.Dictionary<string, object>)
                    UnitTestProject1.UnitTest1.TestMethod_Sigemo() in UnitTest1.cs

            */
            while (true) // Rezolvare prin "catch" la eroare de mai sus (refresh la pagina pana cand driverul identifica elementul)
                        // dupa care click pe icoana "search"
            {
                try
                {
                    driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/button"))
                    .Click(); // click icoana "lupa" (search)
                    
                    break;
                }
                catch (Exception) {
                    driver.Navigate().Refresh(); // Refresh la sectiune si reintroducere BRAND_NAME
                    close_popup();
                    box_search = driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/span/input[2]"));
                    box_search.Clear();
                    box_search.SendKeys(BRAND_NAME);
                    driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/button"))
                    .Click();
                    continue; 
                }
            }
  
            driver.FindElement(By.XPath("/html/body/div[7]/div[2]/div/div/div/div/div/div[1]/div/div/div/div[5]/div/div/div/label/select")).Click(); // click box "latime anvelope"
            driver.FindElement(By.XPath("/html/body/div[7]/div[2]/div/div/div/div/div/div[1]/div/div/div/div[5]/div/div/div/label/select/option[10]")).Click(); //selectare dimeniune latime 225
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("/html/body/div[7]/div[2]/div/div/div/div/div/div[1]/div/div/div/div[3]/div/div/div/label/select/option[3]")).Click(); //selectare inaltime 45

            //var menuOpen = driver.FindElement(By.PartialLinkText("Logare"));
            //menuOpen.Click();
            //driver.Quit();

        }
        [TestMethod]
        public void close_popup()
        {
            // Scop: inchidere popup
            while (true) // inchidere popup
            {
                try
                {
                    var buton_close = driver.FindElement(By.XPath("/html/body/div[7]/div[1]/button")); // Acel popup futa-l draqu'
                    buton_close.Click();
                    break;
                }
                catch (Exception)
                {
                    driver.Navigate().Refresh();
                    continue;
                }

            }
            
        }

        [TestMethod]
        public void test_search()
        {   /// <summary>
            /// Functie cu scopul de a introduce in caseta "search" valorile din ELEM_SEARCH, cu scopul de a observa ce mesaj returneaza...
            /// </summary>
            
            int len_elem_search = ELEM_SEARCH.Length - 1;
            for (int i = 0; i <= len_elem_search;)
            {
                IWebElement search_box = driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/span/input[2]"));
                search_box.Clear();
                search_box.SendKeys(ELEM_SEARCH[i]);
                while (true) // Rezolvare prin "catch" la eroare de mai sus (refresh la pagina pana cand driverul identifica elementul)
                             // dupa care click pe icoana "search"
                {
                    try
                    {
                        driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/button"))
                        .Click(); // click icoana "lupa" (search)
                        break;
                    }
                    catch (Exception)
                    {
                        //driver.Navigate().Refresh(); // Refresh la sectiune si reintroducere ELEM_SEARH[i]
                        //close_popup();
                        search_box = driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/span/input[2]"));
                        search_box.Clear();
                        search_box.SendKeys(ELEM_SEARCH[i]);
                        //driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/button"))
                        //.Click();
                        continue;
                       }
                }
                i++;
            }
            
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

