﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CLanguage.Interpreter;

namespace CLanguage.Syntax
{
    public class ForStatement : Statement
    {
        public Block InitBlock { get; private set; }
        public Expression ContinueExpression { get; private set; }
        public Expression NextExpression { get; private set; }
        public Statement LoopBody { get; private set; }

        public ForStatement (Statement initStatement, Expression continueExpr, Statement body)
        {
            InitBlock = new Block ();
            InitBlock.Statements.Add (initStatement);
            ContinueExpression = continueExpr;
            LoopBody = body;
        }

        public ForStatement (Statement initStatement, Expression continueExpr, Expression nextExpr, Statement body)
        {
            InitBlock = new Block ();
            InitBlock.Statements.Add (initStatement);
            ContinueExpression = continueExpr;
            NextExpression = nextExpr;
            LoopBody = body;
        }

	    public ForStatement(Statement initStatement, Expression continueExpr, Statement body, Block parent, Location startLoc, Location endLoc)
        {
            InitBlock = new Block(startLoc, endLoc);
			InitBlock.Statements.Add (initStatement);
            ContinueExpression = continueExpr;
            LoopBody = body;
        }

		public ForStatement(Statement initStatement, Expression continueExpr, Expression nextExpr, Statement body, Block parent, Location startLoc, Location endLoc)
        {
            InitBlock = new Block(startLoc, endLoc);
			InitBlock.Statements.Add (initStatement);
            ContinueExpression = continueExpr;
            NextExpression = nextExpr;
            LoopBody = body;
        }

        protected override void DoEmit (EmitContext ec)
        {
			InitBlock.Emit (ec);

			var endLabel = ec.DefineLabel ();

			var contLabel = ec.DefineLabel ();
			ec.EmitLabel (contLabel);
			ContinueExpression.Emit (ec);
			ec.EmitCastToBoolean (ContinueExpression.GetEvaluatedCType (ec));
			ec.Emit (OpCode.BranchIfFalse, endLabel);

			LoopBody.Emit (ec);
			NextExpression.Emit (ec);
			ec.Emit (OpCode.Pop);
			ec.Emit (OpCode.Jump, contLabel);

			ec.EmitLabel (endLabel);
        }

        public override bool AlwaysReturns => false;
    }
}
