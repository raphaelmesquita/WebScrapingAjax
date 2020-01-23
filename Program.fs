// Learn more about F# at https://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.Threading
open OpenQA.Selenium
open OpenQA.Selenium.Chrome
open OpenQA.Selenium.Support.UI

//browser.FindElementById("treeList_U").FindElements(By.TagName("img")).First().Click()
//Thread.Sleep(100)
//browser.FindElementById("treeList_R-2674").FindElements(By.TagName("img")).First().Click()
//Thread.Sleep(100)
//browser.FindElementById("treeList_R-2674019").FindElements(By.TagName("img")).First().Click()

let waitUntilId driver idString = 
    let wait = WebDriverWait(driver, TimeSpan.FromSeconds(30.));
    wait.Until(fun w -> w.FindElement(By.Id(idString))) |> ignore
    Thread.Sleep(1500)

[<EntryPoint>]
let main argv =
    let chromeOptions = ChromeOptions()
    chromeOptions.AddArguments("headless")
    chromeOptions.AddUserProfilePreference("download.default_directory", @"C:\temp\");
    use browser = new ChromeDriver(chromeOptions)
    
    let timeouts = browser.Manage().Timeouts()
    timeouts.ImplicitWait <- TimeSpan.FromSeconds(2.)
    timeouts.AsynchronousJavaScript <- TimeSpan.FromSeconds(30.)
    timeouts.PageLoad <- TimeSpan.FromSeconds(30.)
    browser.Manage().Window.Maximize()

    let waitUntil = waitUntilId browser

    browser.Navigate().GoToUrl("http://www14.fgv.br/autenticacao_produtos_licenciados/")
    waitUntil "ctl00_content_btnAuth"

    browser.FindElementById("ctl00_content_txtLogin").SendKeys("SPACOV")
    browser.FindElementById("ctl00_content_txtSenha").SendKeys("12Andrei")
    browser.FindElementById("ctl00_content_btnAuth").Click()
    waitUntil "ctl00_content_rptProduto_ctl00_hpkProduto"

    browser.FindElementById("ctl00_content_rptProduto_ctl00_hpkProduto").Click()
    waitUntil "frmRecadastra"
    
    browser.FindElementById("butRecadastrar").Click()
    
    browser.Navigate().GoToUrl("http://www14.fgv.br/fgvcatalogo20/carregaExcel.aspx?arquivo=Monitor1.0_IPCA_Ponta_012020A544318CC07EC13A9A46157C82CB61E7.XLS&catalogo=2012.1&item=2669&itemClicado=jan/20")
    waitUntil "ctl00_butExportar"
    browser.FindElementById("ctl00_butExportar").Click()
    Thread.Sleep(2000)

    browser.Navigate().GoToUrl("http://www14.fgv.br/fgvcatalogo20/carregaExcel.aspx?arquivo=Monitor1.0_IPCA_012020055DEAFC0EB083500DF0F53D0E151B6D.XLS&catalogo=2012.1&item=2663&itemClicado=jan/20")
    waitUntil "ctl00_butExportar"
    browser.FindElementById("ctl00_butExportar").Click()
    Thread.Sleep(2000)

    browser.Quit()

    0 // return an integer exit code
