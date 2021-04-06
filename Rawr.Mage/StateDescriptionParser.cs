// Generated by TinyPG v1.2 available at www.codeproject.com

using System;
using System.Collections.Generic;

namespace Rawr.Mage.StateDescription
{
    #region Parser

    public partial class Parser 
    {
        private Scanner scanner;
        private ParseTree tree;
        
        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
        }

        public ParseTree Parse(string input, Solver solver)
        {
            tree = new ParseTree(solver);
            return Parse(input, tree);
        }

        public ParseTree Parse(string input, ParseTree tree)
        {
            scanner.Init(input);

            this.tree = tree;
            ParseStart(tree);
            tree.Skipped = scanner.Skipped;

            return tree;
        }

        private void ParseStart(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Start), "Start");
            parent.Nodes.Add(node);


            
            tok = scanner.LookAhead(TokenType.COMPLEMENT, TokenType.STATE, TokenType.BROPEN);
            if (tok.Type == TokenType.COMPLEMENT
                || tok.Type == TokenType.STATE
                || tok.Type == TokenType.BROPEN)
            {
                ParseUnionExpr(node);
            }

            
            tok = scanner.Scan(TokenType.EOF);
            if (tok.Type != TokenType.EOF)
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.EOF.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseUnionExpr(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.UnionExpr), "UnionExpr");
            parent.Nodes.Add(node);


            
            ParseDiffExpr(node);

            
            tok = scanner.LookAhead(TokenType.UNION);
            while (tok.Type == TokenType.UNION)
            {

                
                tok = scanner.Scan(TokenType.UNION);
                if (tok.Type != TokenType.UNION)
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.UNION.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                n = node.CreateNode(tok, tok.ToString() );
                node.Token.UpdateRange(tok);
                node.Nodes.Add(n);

                
                ParseDiffExpr(node);
            tok = scanner.LookAhead(TokenType.UNION);
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseDiffExpr(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.DiffExpr), "DiffExpr");
            parent.Nodes.Add(node);


            
            ParseIntExpr(node);

            
            tok = scanner.LookAhead(TokenType.DIFFERENCE);
            if (tok.Type == TokenType.DIFFERENCE)
            {

                
                tok = scanner.Scan(TokenType.DIFFERENCE);
                if (tok.Type != TokenType.DIFFERENCE)
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DIFFERENCE.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                n = node.CreateNode(tok, tok.ToString() );
                node.Token.UpdateRange(tok);
                node.Nodes.Add(n);

                
                ParseIntExpr(node);
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseIntExpr(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.IntExpr), "IntExpr");
            parent.Nodes.Add(node);


            
            ParseCompExpr(node);

            
            tok = scanner.LookAhead(TokenType.INTERSECTION);
            while (tok.Type == TokenType.INTERSECTION)
            {

                
                tok = scanner.Scan(TokenType.INTERSECTION);
                if (tok.Type != TokenType.INTERSECTION)
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.INTERSECTION.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                n = node.CreateNode(tok, tok.ToString() );
                node.Token.UpdateRange(tok);
                node.Nodes.Add(n);

                
                ParseCompExpr(node);
            tok = scanner.LookAhead(TokenType.INTERSECTION);
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseCompExpr(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.CompExpr), "CompExpr");
            parent.Nodes.Add(node);


            
            tok = scanner.LookAhead(TokenType.COMPLEMENT);
            if (tok.Type == TokenType.COMPLEMENT)
            {
                tok = scanner.Scan(TokenType.COMPLEMENT);
                if (tok.Type != TokenType.COMPLEMENT)
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.COMPLEMENT.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                n = node.CreateNode(tok, tok.ToString() );
                node.Token.UpdateRange(tok);
                node.Nodes.Add(n);
            }

            
            ParseAtom(node);

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseAtom(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Atom), "Atom");
            parent.Nodes.Add(node);

            tok = scanner.LookAhead(TokenType.STATE, TokenType.BROPEN);
            switch (tok.Type)
            {
                case TokenType.STATE:
                    tok = scanner.Scan(TokenType.STATE);
                    if (tok.Type != TokenType.STATE)
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.STATE.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    break;
                case TokenType.BROPEN:

                    
                    tok = scanner.Scan(TokenType.BROPEN);
                    if (tok.Type != TokenType.BROPEN)
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.BROPEN.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);

                    
                    ParseUnionExpr(node);

                    
                    tok = scanner.Scan(TokenType.BRCLOSE);
                    if (tok.Type != TokenType.BRCLOSE)
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.BRCLOSE.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    break;
                default:
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                    break;
            }

            parent.Token.UpdateRange(node.Token);
        }


    }

    #endregion Parser
}
