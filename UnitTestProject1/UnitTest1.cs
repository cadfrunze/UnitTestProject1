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
        public  string[] ELEM_SEARCH = {"", "bla bla"};
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
            //Thread.Sleep(5000);
            //driver.Navigate().Refresh();
            driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[2]/ul/li[1]/a")).Click();
            Thread.Sleep(5);
            close_popup();
            Thread.Sleep(2000);
            // text_box search, introducere var BRAND in boxa "search"
            IWebElement box_search = driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/span/input[2]"));
            box_search.Click(); 
            box_search.Clear();
            box_search.SendKeys(BRAND_NAME); // adaugare in text box variabila BRAND
            status_net();
            Thread.Sleep(2000);

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
                    box_search.Click();
                    box_search.Clear();
                    box_search.SendKeys(BRAND_NAME);
                    //driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/button"))
                    //.Click();
                    continue; 
                }
            }
  
            driver.FindElement(By.XPath("/html/body/div[7]/div[2]/div/div/div/div/div/div[1]/div/div/div/div[5]/div/div/div/label/select")).Click(); // click box "latime anvelope"
            driver.FindElement(By.XPath("/html/body/div[7]/div[2]/div/div/div/div/div/div[1]/div/div/div/div[5]/div/div/div/label/select/option[10]")).Click(); //selectare dimeniune latime 225
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("/html/body/div[7]/div[2]/div/div/div/div/div/div[1]/div/div/div/div[3]/div/div/div/label/select/option[3]")).Click(); //selectare inaltime 45
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("/html/body/div[7]/div[2]/div/div/div/div/div/div[1]/div/div/div/div[4]/div/div/div/label/select/option[2]")).Click();
            Thread.Sleep(5000);
            
            // Cautare, returnare colectie de elemente de tipul IWebElement (ATENTIE!!!) 
            IReadOnlyCollection<IWebElement> elemente_iarna = driver.FindElements(By.PartialLinkText("iarna"));


            // Afisare in consola....
            foreach (IWebElement element in elemente_iarna){ // hmm...in python = for element in elemente print(element.text)
                Console.WriteLine(element.Text);
            }
            Thread.Sleep(5000);
            //var menuOpen = driver.FindElement(By.PartialLinkText("Logare"));
            //menuOpen.Click();
            driver.Quit();
            //Console.WriteLine(USER_SIGEMO);

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
                search_box.Click();
                search_box.Clear();
                search_box.SendKeys(ELEM_SEARCH[i]);
                while (true) // Rezolvare prin "catch" la eroare de mai sus (refresh la pagina pana cand driverul identifica elementul)
                             // dupa care click pe icoana "search"
                {
                    try
                    {
                        driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/button"))
                        .Click(); // click icoana "lupa" (search)
                        driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/button"))
                        .Click();
                        break;
                    }
                    catch (Exception)
                    {
                        driver.Navigate().Refresh(); // Refresh la sectiune si reintroducere ELEM_SEARH[i]
                        //close_popup();
                        search_box = driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/span/input[2]"));
                        search_box.Click();
                        search_box.Clear();
                        search_box.SendKeys(ELEM_SEARCH[i]);
                        //driver.FindElement(By.XPath("/html/body/div[7]/header/div[1]/div[3]/div[3]/div/div/div/button"))
                        //.Click();
                        continue;
                       }
                }
                i++;
                Thread.Sleep(2000);
            }
            
            
        }

        /*public void close_chat()
        { // Inchidere dialog chat
            while (true)
            {
                try { 
                    driver.FindElement(By.ClassName("x1i10hfl x1qjc9v5 xjbqb8w xjqpnuy xa49m3k xqeqjp1 x2hbi6w x13fuv20 xu3j5b3 x1q0q8m5 x26u7qi x972fbf xcfux6l x1qhh985 xm0m39n x9f619 x1ypdohk xdl72j9 x2lah0s xe8uvvx xdj266r x11i5rnm xat24cr x1mh8g0r x2lwn1j xeuugli xexx8yu x4uap5 x18d9i69.xkhd6sd.x1n2onr6.x16tdsg8.x1hl2dhg.xggy1nq.x1ja2u2z.x1t137rt.x1o1ewxj.x3x9cwd.x1e5q0jg.x13rtm0m.x3nfvp2.x1q0g3np.x87ps6o.x1lku1pv.x1a2a7pz")).Click();
                    break;
                }
                catch (Exception) { continue; }    
                
            }
        }*/

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
                catch (Exception) {
                    driver.Navigate().Refresh();
                    continue; }
            
        }   
    }
}

