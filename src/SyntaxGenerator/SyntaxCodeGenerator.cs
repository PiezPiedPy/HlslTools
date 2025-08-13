using Microsoft.CodeAnalysis;
using SyntaxGenerator.Model;
using SyntaxGenerator.Writer;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SyntaxGenerator
{
    [Generator]
    public class SyntaxCodeGenerator : IIncrementalGenerator
    {
        //public void Initialize(GeneratorInitializationContext context)
        //{
        //    // No initialization required
        //}

        //public void Execute(GeneratorExecutionContext context)
        //{
        //    var fullSyntaxPath = context.AdditionalFiles[0].Path;

        //    FileStream fs = null;

        //    try
        //    {
        //        fs = File.OpenRead(fullSyntaxPath);
        //    }
        //    catch (Exception ex) when (ex is DirectoryNotFoundException || ex is FileNotFoundException)
        //    {
        //        var fnf = Diagnostic.Create("SCG_FileNotFound", "SyntaxCodeGenerator", $"The syntax file at {fullSyntaxPath} was not found.", DiagnosticSeverity.Error, DiagnosticSeverity.Error, true, 0);
        //        context.ReportDiagnostic(fnf);
        //        return;
        //    }

        //    using (fs)
        //    {
        //        var reader = XmlReader.Create(fs, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit });
        //        var serializer = new XmlSerializer(typeof(Tree));
        //        Tree tree = (Tree)serializer.Deserialize(reader);

        //        context.CancellationToken.ThrowIfCancellationRequested();

        //        var stringBuilder = new StringBuilder();
        //        var writer = new StringWriter(stringBuilder);
        //        SourceWriter.WriteAll(writer, tree);

        //        context.AddSource("Syntax.generated.cs", stringBuilder.ToString());
        //    }
        //}

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Register the syntax file as an additional file
            var syntaxFiles = context.AdditionalTextsProvider
                .Where(static file => file.Path.EndsWith(".xml")); // Adjust the filter as needed

            // Combine with compilation provider to get access to compilation data if needed
            var compilationAndFiles = context.CompilationProvider.Combine(syntaxFiles.Collect());

            context.RegisterSourceOutput(compilationAndFiles, (productionContext, sourceContext) =>
            {
                var (compilation, files) = sourceContext;

                var mainFile = files.FirstOrDefault();
                if (mainFile == null)
                {
                    productionContext.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "SCG_NoFilesFound",
                            "No syntax files found",
                            "No syntax definition files were found",
                            "SyntaxCodeGenerator",
                            DiagnosticSeverity.Error,
                            true),
                        Location.None));
                    return;
                }
                try
                {
                    var content = mainFile.GetText();
                    if (content == null)
                    {
                        productionContext.ReportDiagnostic(Diagnostic.Create(
                            new DiagnosticDescriptor(
                                "SCG_FileEmpty",
                                "File is empty",
                                $"The syntax file at {mainFile.Path} is empty",
                                "SyntaxCodeGenerator",
                                DiagnosticSeverity.Error,
                                true),
                            Location.None));
                        return;
                    }

                    using var reader = XmlReader.Create(new StringReader(content.ToString()),
                        new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit });

                    var serializer = new XmlSerializer(typeof(Tree));
                    if (serializer.Deserialize(reader) is not Tree tree)
                    {
                        productionContext.ReportDiagnostic(Diagnostic.Create(
                            new DiagnosticDescriptor(
                                "SCG_InvalidFormat",
                                "Invalid file format",
                                $"The syntax file at {mainFile.Path} could not be deserialized",
                                "SyntaxCodeGenerator",
                                DiagnosticSeverity.Error,
                                true),
                            Location.None));
                        return;
                    }

                    productionContext.CancellationToken.ThrowIfCancellationRequested();

                    var stringBuilder = new StringBuilder();
                    using (var writer = new StringWriter(stringBuilder))
                    {
                        SourceWriter.WriteAll(writer, tree);
                    }

                    productionContext.AddSource("Syntax.generated.cs", stringBuilder.ToString());
                }
                catch (XmlException xmlEx)
                {
                    productionContext.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "SCG_XmlError",
                            "XML parsing error",
                            $"Error parsing XML in {mainFile.Path}: {xmlEx.Message}",
                            "SyntaxCodeGenerator",
                            DiagnosticSeverity.Error,
                            true),
                        Location.None,
                        xmlEx.LineNumber,
                        xmlEx.LinePosition));
                }
                catch (Exception ex)
                {
                    productionContext.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "SCG_GeneralError",
                            "General error",
                            $"Error processing {mainFile.Path}: {ex.Message}",
                            "SyntaxCodeGenerator",
                            DiagnosticSeverity.Error,
                            true),
                        Location.None));
                }
            });
        }
    }
}

