// Learn more about F# at https://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.Threading
open System.Net
open ScrapySharp.Network
open ScrapySharp.Html
open System.Linq
open HtmlAgilityPack
open ScrapySharp.Extensions
open OpenQA.Selenium
open OpenQA.Selenium.Chrome
open OpenQA.Selenium.Support.UI

//let browser = ScrapingBrowser()
//browser.AllowAutoRedirect <- true
//browser.AllowMetaRedirect <- true
//let loginPage = browser.NavigateToPage(Uri("http://www14.fgv.br/autenticacao_produtos_licenciados/"))
//let loginForm = loginPage.FindFormById("aspnetForm")
//loginForm.["ctl00$content$txtLogin"] <- "SPACOV"
//loginForm.["ctl00$content$txtSenha"] <- "12Andrei"
//loginForm.Method <- HttpVerb.Post
//let produtosPage = loginForm.Submit()
//let oi = produtosPage.Find("a", By.Id("ctl00_content_rptProduto_ctl00_hpkProduto")).Single().Attributes.["href"].Value
//let oi2 = produtosPage.Browser.NavigateToPage(Uri(oi))
//let recadastrarPage = produtosPage.FindLinks(By.Id("ctl00_content_rptProduto_ctl00_hpkProduto")).Single().Click()
////let link = produtosPage.FindLinks(By.Id("ctl00_content_rptProduto_ctl00_hpkProduto")).Single()
////
////let oi = client.UploadString("http://www14.fgv.br/autenticacao_produtos_licenciados/", "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwULLTE2OTg0MTkzNDMPZBYCZg9kFgICAw9kFgICBQ9kFgICAw9kFgICAQ9kFgICCQ8PFgIeC05hdmlnYXRlVXJsBTdodHRwOi8vd3d3MTQuZmd2LmJyL2ZndmRhZG9zMjAvZGVmYXVsdC5hc3B4P0NvbnZpZGFkbz1TZGRk3U0G5rXu1LdM2QULcuPad5Vttigo3y4h%2BcFA9l4lEIA%3D&__VIEWSTATEGENERATOR=AC2E16B1&__EVENTVALIDATION=%2FwEdAAT7WG9VlM9Qk0A%2FJ%2FKOVqi2bqG%2Bmov7l7cKAtWu3EPVw%2FHdEwtd8Z7PlDicq6eoDUoVpagxvgmbMiDyNGGPa1FoYB4Q87Dd9O1hj%2F3U2BTTlowGOgkwXWT7bSJGGbH0hEA%3D&ctl00%24content%24txtLogin=SPACOV&ctl00%24content%24txtSenha=12Andrei&ctl00%24content%24btnAuth=Entrar")
////let oi2 = client.DownloadString("http://www14.fgv.br/fgvcatalogo20/default.aspx?origem=P&&produto=MONITOR&UserId=8A7C823325272A9B01252756CB016533&token=e4ec0c3a-f9cc-4a1a-bbdc-5377c4249262")


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
