using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using McMaster.Extensions.CommandLineUtils;
using System.Globalization;
using System.Text;

namespace HtmlToXml
{
    class Program
    {
        static int Main(string[] args)
        {

            var app = new CommandLineApplication();

            app.HelpOption();
            var inputFilepath = app.Option("-f|--inputFile <PATH>", "A file with `html` input.", CommandOptionType.SingleValue);
            var outputFilepath = app.Option("-o|--outputFile <PATH>", "The path of a filename for writing xml output.", CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                // Read the html from the specified file, or from stdin if none is specified.
                var html = inputFilepath.HasValue() ? System.IO.File.ReadAllText(inputFilepath.Value())
                    : System.Console.IsInputRedirected ? System.Console.In.ReadToEnd()
                    : null;

                var converter = new HtmlConverter();
                var xml = converter.Convert(html);

                // Write the xml to the specified file, or to stdout if none is specified.
                if (outputFilepath.HasValue())
                    System.IO.File.WriteAllText(outputFilepath.Value(), xml);
                else if (System.Console.IsOutputRedirected)
                    System.Console.Out.Write(xml);
                else
                    System.Console.Out.WriteLine(xml);

                return 0;
            });

            return app.Execute(args);
        }
    }


}


