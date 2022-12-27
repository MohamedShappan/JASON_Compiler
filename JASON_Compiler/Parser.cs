﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{
    public class Node
    {
        public List<Node> children = new List<Node>();
        public Token token;
    }

    class ParsingPhase
    {
        public static List<KeyValuePair<string, string>> ParserErrors = new List<KeyValuePair<string, string>>();
        private static List<Token> Tokens = new List<Token>();
        private static bool thereisREturnStatement = false;
        private static int index = 0;
        // private static bool scanforStablePoint = false; 
        public static Node Parse(List<Token> Tokens_)
        {
            Node root = new Node();
            root.token = new Token();
            root.token.lex = "program";
            Tokens = Tokens_;
            root = program();

            return root;
        }
        public static Node match(Token_Class tokentype)
        {
            Node node = new Node();
            if (index < Tokens.Count)
                if (tokentype == Tokens[index].tokentype)
                {
                    node.token = Tokens[index];
                    index++;

                    return node;
                }


            return null;
        }
        // 1
        public static Node program()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Program.ToString();
            Node functionStatementNode = functionStatement();
            if (functionStatementNode != null)
            {
                node.children.Add(functionStatementNode);
                node.children.Add(program());
                return node;
            }
            functionStatementNode = mainFuction();
            if (functionStatementNode != null)
            {
                node.children.Add(functionStatementNode);
                return node;
            }
            return null;
        }
        //2
        public static Node mainFuction()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Main_Fuction.ToString();
            Node datatypeNode = dataType();
            if (datatypeNode != null)
            {
                node.children.Add(datatypeNode);
                node.children.Add(match(Token_Class.Main));
                node.children.Add(match(Token_Class.LeftParentheses));
                node.children.Add(match(Token_Class.RightParentheses));
                if (node.children[1] == null || node.children[2] == null || node.children[3] == null)
                {
                    addError("Main () ");
                }
                node.children.Add(functionBody());

                return node;
            }

            return null;
        }
        public static Node functionStatement()
        {
            
            if (Tokens[index + 1].tokentype == Token_Class.Main)
            {
                return null;
            }
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Fuction_Statement.ToString();

            Node decalartionNode = functionDeclaration();
            if (decalartionNode != null)
            {
                node.children.Add(decalartionNode);
                node.children.Add(functionBody());
                return node;
            }
            return null;
        }
        public static Node functionDeclaration()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Fuction_Declaration.ToString();
            Node datatypeNode = dataType();
            if (datatypeNode != null)
            {
                node.children.Add(datatypeNode);
                node.children.Add(functionName());
                node.children.Add(match(Token_Class.LeftParentheses));
                node.children.Add(parameters());
                node.children.Add(match(Token_Class.RightParentheses));
                if (node.children[1] == null || node.children[2] == null || node.children[3] == null || node.children[4] == null)
                {
                    addError("DataType FunctionName (no_Para) ");
                }
                return node;
            }
            return null;

        }
        public static Node functionBody()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Function_Body.ToString();
            Node leftNode = match(Token_Class.LeftBraces);
            if (leftNode != null)
            {
                node.children.Add(leftNode);
                node.children.Add(statements());
                node.children.Add(returnStatement());
                node.children.Add(match(Token_Class.RightBraces));
                if (node.children[1] == null || node.children[2] == null || node.children[3] == null || node.children[0] == null)
                {
                    addError("{ Statments }");
                }
                if (!thereisREturnStatement && node.children[2] == null)
                {
                    addError("The Function must has a return statement ");
                    MessageBox.Show("The Function must has a return statement ");
                }
                return node;
            }
            return null;

        }
        public static Node statements()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Statements.ToString();
            Node statementNode = statement();
            if (statementNode != null)
            {
                node.children.Add(statementNode);
                node.children.Add(statements());
                return node;
            }
            return null;
        }
        public static Node statement()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Statement.ToString();
            Node statmentNode = repeatStatement();
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                return node;
            }
            statmentNode = ifStatement();
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                return node;
            }
            statmentNode = readStatement();
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                return node;
            }
            statmentNode = writeStatement();
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                return node;
            }
            statmentNode = declarationStatement();
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                return node;
            }
            statmentNode = assignmentStatement();
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                return node;
            }
            statmentNode = commentStatement();
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                return node;
            }
            statmentNode = returnStatement();
            if (statmentNode != null)
            {
                thereisREturnStatement = true;
                node.children.Add(statmentNode);
                return node;
            }

            return null;
        }
        public static Node parameter()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Parameter.ToString();
            Node dataTypeNode = dataType();
            if (dataTypeNode != null)
            {
                node.children.Add(dataTypeNode);
                node.children.Add(identifier());
                if (node.children[0] == null || node.children[1] == null)
                {
                    addError("DataType identifier , ..... ");
                }
                return node;
            }
            return null;
        }
        public static Node parameters()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Parameters.ToString();
            Node parameterNode = parameter();
            if (parameterNode != null)
            {
                node.children.Add(parameterNode);
                node.children.Add(parametersCont());
                return node;
            }
            return null;
        }
        public static Node parametersCont()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Parameters_Cont.ToString();
            Node CommaNode = comma();
            if (CommaNode != null)
            {
                node.children.Add(CommaNode);
                node.children.Add(parameter());
                node.children.Add(parametersCont());
                if (node.children[1] == null)
                {
                    addError(" You must have another parameter ");
                }
                return node;
            }
            return null;
        }
        public static Node functionName()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Fuction_Name.ToString();
            Node identifierNode = identifier();
            if (identifierNode != null)
            {
                node.children.Add(identifierNode);
                if (node.children[0] == null)
                {
                    addError("it must be identifier ");
                }
                return node;
            }
            return null;
        }

        public static Node repeatStatement()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Repeat_Statement.ToString();
            Node repeatNode = match(Token_Class.Repeat);
            if (repeatNode != null)
            {
                node.children.Add(repeatNode);
                node.children.Add(statements());
                node.children.Add(match(Token_Class.Until));
                node.children.Add(conditionStatement());
                if (node.children[2] == null || node.children[3] == null)
                {
                    addError("there're something missing");
                }
                return node;
            }
            return null;

        }
        public static Node elseStatement()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Else_Statement.ToString();
            Node elseNode = match(Token_Class.Else);
            if (elseNode != null)
            {
                node.children.Add(elseNode);
                node.children.Add(statements());
                node.children.Add(match(Token_Class.End));
                if (node.children[2] == null)
                {
                    addError("End is missing");
                }
                return node;
            }
            return null;
        }
        public static Node elseIfStatement()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Else_Statement.ToString();
            Node elseifNode = match(Token_Class.Elseif);
            if (elseifNode != null)
            {
                node.children.Add(elseifNode);
                node.children.Add(conditionStatement());
                node.children.Add(match(Token_Class.Then));
                node.children.Add(statements());
                node.children.Add(endIfStatement());
                if (node.children[2] == null)
                {
                    addError("The Format must be then stat.. end || else... ");
                }
                return node;
            }
            return null;
        }
        public static Node ifStatement()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.If_Statement.ToString();
            Node ifNode = match(Token_Class.If);
            if (ifNode != null)
            {
                node.children.Add(ifNode);
                node.children.Add(conditionStatement());
                node.children.Add(match(Token_Class.Then));
                node.children.Add(statements());
                node.children.Add(endIfStatement());
                if (node.children[2] == null)
                {
                    addError("The Format must be then stat.. end || else... ");
                }
                return node;

            }
            return null;

        }
        public static Node endIfStatement()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.EndIf_Statement.ToString();
            Node elseStatNode = elseStatement();
            if (elseStatNode != null)
            {
                node.children.Add(elseStatNode);
                node.children.Add(elseIfStatement());
                node.children.Add(match(Token_Class.End));

                return node;
            }
            elseStatNode = elseIfStatement();
            if (elseStatNode != null)
            {
                node.children.Add(elseStatNode);

                return node;
            }
            elseStatNode = match(Token_Class.End);
            if (elseStatNode != null)
            {
                node.children.Add(elseStatNode);
                return node;
            }
            return null;
        }
        public static Node orOP()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Or_Operation.ToString();
            Node orNode = match(Token_Class.Or);
            if (orNode != null)
            {
                node.children.Add(orNode);
                return node;
            }

            return null;
        }
        public static Node andOP()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.And_Operation.ToString();
            Node andNode = match(Token_Class.And);
            if (andNode != null)
            {
                node.children.Add(andNode);
                return node;
            }
            return null;

        }
        public static Node condition()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Condition.ToString();
            Node expreNode = expression();
            if (expreNode != null)
            {
                node.children.Add(expreNode);
                node.children.Add(conditionOperator());
                node.children.Add(expression());
                if (node.children[1] == null || node.children[2] == null)
                {
                    addError("The Format must be Expression Operator Expression ");
                }
                return node;
            }
            return null;

        }

        public static Node conditionTerm()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Condition_Term.ToString();
            Node CondationNode = condition();
            if (CondationNode != null)
            {
                node.children.Add(CondationNode);
                node.children.Add(conditionTermDash());
                return node;
            }
            return null;
        }
        public static Node conditionTermDash()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Condition_Term_Dash.ToString();
            Node andNode = andOP();
            if (andNode != null)
            {
                node.children.Add(andNode);
                node.children.Add(condition());
                node.children.Add(conditionTermDash());
            }

            return node;
        }
        public static Node conditionStatement()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Condition_Statement.ToString();
            Node conditionNode = condition();
            if (conditionNode != null)
            {
                node.children.Add(conditionNode);
                node.children.Add(orOP());
                node.children.Add(conditionTerm());

                return node;
            }
            else
            {
                conditionNode = conditionTerm();
                if (conditionNode != null)
                {
                    node.children.Add(conditionNode);
                }

            }

            return node;
        }
        public static Node conditionOperator()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Condition_Operator.ToString();
            Node OpNode = match(Token_Class.LessThan);
            if (OpNode != null)
            {
                node.children.Add(OpNode);
                return node;
            }
            OpNode = match(Token_Class.GreaterThan);
            if (OpNode != null)
            {
                node.children.Add(OpNode);
                return node;
            }
            OpNode = match(Token_Class.Equal);
            if (OpNode != null)
            {
                node.children.Add(OpNode);
                return node;
            }
            OpNode = match(Token_Class.NotEqual);
            if (OpNode != null)
            {
                node.children.Add(OpNode);
                return node;
            }

            return null;
        }
        public static Node returnStatement()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Return_Statement.ToString();
            Node returnNode = match(Token_Class.Return);
            if (returnNode != null)
            {
                node.children.Add(returnNode);
                node.children.Add(expression());
                node.children.Add(match(Token_Class.SemiColon));
                if (node.children[2] == null)
                {
                    addError("SemiColon is missed");
                }
                return node;
            }
            return null;
        }
        public static Node readStatement()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Read_Statement.ToString();

            Node readNode = match(Token_Class.Read);
            if (readNode != null)
            {
                node.children.Add(readNode);
                node.children.Add(identifier());
                Node simNode = match(Token_Class.SemiColon);
                node.children.Add(simNode);
                if (node.children[2] == null)
                {
                    addError("SemiColon is missed");
                }
                return node;
            }
            return null;
        }

        public static Node writeStatement()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Write_Statement.ToString();
            Node writeNode = match(Token_Class.Write);
            if (writeNode != null)
            {
                node.children.Add(writeNode);
                node.children.Add(writeStatementCont());
                node.children.Add(match(Token_Class.SemiColon));
                if (node.children[2] == null)
                {
                    addError("SemiColon is missed");
                }
                return node;

            }
            return null;
        }
        public static Node writeStatementCont()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Write_Statement_Cont.ToString();
            Node endlNode = match(Token_Class.Endl);
            if (endlNode != null)
            {
                node.children.Add(endlNode);
                return node;
            }
            else
            {
                Node expreNode = expression();
                if (expreNode != null)
                {
                    node.children.Add(expreNode);

                    return node;
                }
            }

            return null;
        }

        public static Node declarationStatement()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Declaration_Statement.ToString();
            Node dataTypeNode = dataType();
            if (dataTypeNode != null)
            {
                node.children.Add(dataTypeNode);
                node.children.Add(identifier());
                node.children.Add(declarationStatementCont());
                node.children.Add(match(Token_Class.SemiColon));
                if (node.children[1] == null || node.children[3] == null)
                {
                    addError("The Format must be dataType identifier :=  value ; ");
                }
                return node;
            }
            return null;

        }
        public static Node declarationStatementCont()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Declaration_Statement.ToString();
            Node commaNode = declarationStatementCase();
            if (commaNode != null)
            {
                node.children.Add(commaNode);
                node.children.Add(declarationStatementCont());
                return node;
            }
            return null;
        }
        public static Node declarationStatementCase()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Declaration_Statement_Case.ToString();
            Node commaNode = comma();
            if (commaNode != null)
            {
                node.children.Add(commaNode);
                node.children.Add(identifier());
                return node;
            }
            Node assignNode = match(Token_Class.Assign);
            if (assignNode != null)
            {
                node.children.Add(assignNode);
                node.children.Add(expression());
                return node;
            }
            return null;
        }
        public static Node comma()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Comma.ToString();
            Node CommaNode = match(Token_Class.Comma);
            if (CommaNode != null)
            {
                node.children.Add(CommaNode);
                return node;
            }
            return null;
        }
        public static Node dataType()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.DataType.ToString();

            Node intNode = match(Token_Class.Int);

            if (intNode != null)
            {
                node.children.Add(intNode);
                return node;
            }
            Node stringNode = match(Token_Class.String);
            if (stringNode != null)
            {
                node.children.Add(stringNode);
                return node;

            }
            Node floatNode = match(Token_Class.Float);
            if (floatNode != null)
            {
                node.children.Add(floatNode);
                return node;
            }
            Node charNode = match(Token_Class.Char);
            if (charNode != null)
            {
                node.children.Add(charNode);
                return node;

            }

            return null;
        }
        public static Node assignmentStatement()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Assignment_Statement.ToString();

            Node identifierNode = identifier();
            if (identifierNode != null)
            {
                node.children.Add(identifierNode);
                node.children.Add(match(Token_Class.Assign));
                node.children.Add(expression());
                node.children.Add(match(Token_Class.SemiColon));
                if (node.children[1] == null || node.children[3] == null)
                {
                    addError("The Format must be  identifier :=  expression ; ");
                }
                return node;
            }

            return null;
        }
        public static Node expression()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Expression.ToString();
            Node termNode = term();
            if (termNode != null)
            {
                node.children.Add(termNode);
                node.children.Add(expressionDash());
                return node;
            }
            return null;
        }
        public static Node expressionDash()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Expression_Dash.ToString();
            Node addopNode = addOp();
            if (addopNode != null)
            {
                node.children.Add(addopNode);
                node.children.Add(term());
                node.children.Add(expressionDash());
                return node;
            }
            return null;
        }
        public static Node addOp()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.And_Operation.ToString();
            Node opNode = match(Token_Class.Plus);
            if (opNode != null)
            {
                node.children.Add(opNode);
                return node;
            }
            opNode = match(Token_Class.Minus);
            if (opNode != null)
            {
                node.children.Add(opNode);
                return node;
            }

            return null;
        }
        public static Node mulOp()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Mul_Operation.ToString();
            Node timeNode = match(Token_Class.Times);
            if (timeNode != null)
            {
                node.children.Add(timeNode);
                return node;
            }
            timeNode = match(Token_Class.Division);
            if (timeNode != null)
            {
                node.children.Add(timeNode);

                return node;
            }
            return null;
        }

        public static Node term()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Term.ToString();
            Node factorNode = factor();
            if (factorNode != null)
            {
                node.children.Add(factorNode);
                node.children.Add(termDash());
                return node;
            }
            return null;

        }
        public static Node termDash()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.TermDash.ToString();

            Node mulopNode = mulOp();
            if (mulopNode != null)
            {
                node.children.Add(mulopNode);
                node.children.Add(factor());
                node.children.Add(termDash());
                return node;
            }
            return null;
        }
        public static Node factor()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Factor.ToString();
            Node statmentNode = number();
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                return node;
            }
            statmentNode = string_();
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                return node;

            }
            statmentNode = functionCall();
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                return node;
            }
            statmentNode = match(Token_Class.LeftParentheses);
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                node.children.Add(expression());
                node.children.Add(match(Token_Class.RightParentheses));
                return node;
            }
            statmentNode = identifier();
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                return node;
            }
            return null;
        }
        public static Node functionPart()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Function_Part.ToString();
            Node statmentNode = match(Token_Class.LeftParentheses);
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                node.children.Add(functionArguments());
                node.children.Add(match(Token_Class.RightParentheses));
                return node;
            }
            return null;
        }
        public static Node functionArguments()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Function_Arguments.ToString();
            Node statmentNode = expression();
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                node.children.Add(functionArgumentsCont());
                return node;
            }
            return null;
        }
        public static Node functionArgumentsCont()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Function_Arguments_Cont.ToString();
            Node statmentNode = match(Token_Class.Comma);
            if (statmentNode != null)
            {
                node.children.Add(statmentNode);
                node.children.Add(identifier());
                node.children.Add(functionArgumentsCont());
                return node;
            }
            return null;
        }
        public static Node functionCall()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Function_Call.ToString();
            Node identifierNode = identifier();
            if (identifierNode != null)
            {
                node.children.Add(identifierNode);
                node.children.Add(functionPart());
                return node;
            }
            return null;
        }
        public static Node identifier()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Identifier.ToString();

            Node readNode = match(Token_Class.Identifier);
            if (readNode != null)
            {
                node.children.Add(readNode);
                return node;
            }
            return null;
        }

        public static Node letterDigitComp()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.letter_DigitComp.ToString();
            node.children.Add(string_());
            return node;
        }
        public static Node letter()
        {
            Node node = new Node();
            return node;
        }
        public static Node digit()
        {
            Node node = new Node();
            return node;
        }
        public static Node digits()
        {
            Node node = new Node();
            
            return node;
        }
        public static Node commentStatement()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Comment_Statement.ToString();
            Node commentNode = match(Token_Class.Comment);
            if (commentNode != null)
            {
                node.children.Add(commentNode);
                return node;
            }
            return null;
        }
        public static Node reservedKeywords()
        {
            Node node = new Node();
        
            return node;
        }

        public static Node string_()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.String.ToString();
            Node stringNode = match(Token_Class.String);
            if (stringNode != null)
            {
                node.children.Add(stringNode);
                return node;
            }
            return null;
        }
        public static Node number()
        {
            Node node = new Node();
            node.token = new Token();
            node.token.lex = Token_Class.Number.ToString();
            Node numberNode = match(Token_Class.Number);
            if (numberNode != null)
            {
                node.children.Add(numberNode);
                return node;
            }
            return null;

        }


        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.token == null)
                return null;
            TreeNode tree = new TreeNode(root.token.lex);
            if (root.children.Count == 0)
                return tree;
            foreach (Node child in root.children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
        public static void clear()
        {
            Tokens = new List<Token>();
            index = 0;
        }
        public static void addError(string expected)
        {
            Errors.Error_List.Add(expected);
        }

    }
}