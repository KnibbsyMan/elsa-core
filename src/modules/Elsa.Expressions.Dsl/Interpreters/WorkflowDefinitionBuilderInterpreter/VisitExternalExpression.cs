using Elsa.Expressions.Models;
using Elsa.Workflows;

namespace Elsa.Expressions.Dsl.Interpreters;

public partial class WorkflowDefinitionBuilderInterpreter
{
    public override IWorkflowBuilder VisitExpressionMarker(ElsaParser.ExpressionMarkerContext context)
    {
        // var language = context.ID();
        // var expressionContent = context.expressionContent().GetText();
        //
        // // TODO: Determine actual expression type based on specified language.
        // var expression = new JavaScriptExpression(expressionContent);
        // var expressionReference = new JavaScriptExpressionBlockReference(expression);
        // var externalReference = new ExternalExpressionReference(expression, expressionReference);
        // _expressionValue.Put(context, externalReference);
        // _expressionValue.Put(context.Parent, externalReference);

        return DefaultResult;
    }
}

public record ExternalExpressionReference(Expression Expression, MemoryBlockReference BlockReference);