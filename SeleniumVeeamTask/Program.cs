using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;
using System.Collections.Generic;
using NUnit.Framework;

namespace SeleniumVeeamTask
{
    class Program
    {
        private static ChromeDriver driver;
        private static string url = "https://careers.veeam.com/vacancies";
        private static string pathDriver = @"C:\Users\rev-i\Downloads";

        static void Main()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());

            TestLocationResultForCountry("en");
        }

        public static void TestLocationResultForCountry(string text)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--lang=" + text);
            driver = new ChromeDriver(pathDriver, options);
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl(url);

            SetValueForParametr("department-toggler", "Research & Development");

            SetValueForParametr("city-toggler", "All countries");

            IWebElement btn = null;
            try
            {
                btn = driver.FindElement(By.XPath("//div[@class = 'form-group d-flex flex-column flex-lg-row justify-content-center justify-content-md-end align-items-md-center']" +
                   "//button[@class='btn btn-outline-success']"));
            }
            catch(Exception ex) { Assert.Fail(ex.Message); }
            Assert.IsNotNull(btn);

            btn.Click();

            CompareResult(22);
        }

        public static void SetValueForParametr(string parametrId, string text)
        {
            IWebElement selectElement = null;
            try
            {
                selectElement = driver.FindElement(By.Id(parametrId));
            }
            catch (Exception ex) { Assert.Fail(ex.Message); }

            Assert.IsNotNull(selectElement);

            selectElement.Click();

            IWebElement element = null;
            try
            {
                element = selectElement.FindElement(By.XPath("//*[text()='"+ text +"']"));
            }
            catch (Exception ex) { Assert.Fail(ex.Message); }
            Assert.IsNotNull(element);

            ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView(true);", element);

            IList<IWebElement> optionsList = null;
            try
            {
                optionsList = driver.FindElements(By.CssSelector("a[class*='dropdown-item']"));
            }
            catch (Exception ex) { Assert.Fail(ex.Message); }
            Assert.IsNotNull(optionsList);

            foreach (IWebElement option in optionsList)
                if (option.Text.Equals(text))
                    option.Click();
        }

        public static void CompareResult(int expactedResult)
        {
            var vacanciesList = driver.FindElements(By.XPath("//div[@class='bg-light position-relative']" +
               "//div[@class='container container-spacer']" +
               "//div[@class='row d-none d-md-block']" +
               "//div[@class='col-12']" +
               "//div[@class='card-columns card-columns__card-columns-vacancies']" +
               "//div[@class='d-none d-lg-block']" +
               "//a[@class='card card-md-45 card-no-hover card--shadowed']"));

            Assert.IsNotNull(vacanciesList.Count);

            Assert.AreEqual(vacanciesList.Count, expactedResult);
        }
        
    }
}
