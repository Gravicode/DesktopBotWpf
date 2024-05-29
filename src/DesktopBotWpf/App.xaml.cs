using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Configuration;
using System.Data;
using System.Windows;

namespace DesktopBotWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IAlertService? AlertService { get; set; }
        public App()
        {
            Setup();
        }

        void Setup()
        {
            var model = "gpt-4o";
            var key = ConfigurationManager.AppSettings["OpenAIKey"];
            var org = ConfigurationManager.AppSettings["OpenAIOrg"];
            Kernel kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(modelId: model, apiKey: key, orgId: org)
            .Build();
            //.AddOpenAIChatCompletion("phi-3-mini-q4", new Uri("http://localhost:5272/v1/chat/completions"), null, serviceId: "localmodel")
            kernel.Plugins.AddFromType<OrganizeDesktopPlugin>();
            var chatCompletion = kernel.GetRequiredService<IChatCompletionService>();
            ObjectContainer.Register<Kernel>(kernel);
            ObjectContainer.Register<IAlertService>(new AlertService());
            var manager = new ChatManager(kernel, chatCompletion);
            ObjectContainer.Register<ChatManager>(manager);
            ObjectContainer.Register<ChatHistoryViewModel> (new ChatHistoryViewModel(manager));
        }
       
    }

}
