﻿using fesch.Services.Exceptions;
using Gurobi;
using System;

namespace fesch.Services.Scheduler
{
    public class Scheduler
    {
        private static Scheduler instance = null;
        public static Scheduler Service
        {
            get
            {
                if (instance == null)
                {
                    instance = new Scheduler();
                }
                return instance;
            }
        }
        private GRBModel model;
        private Scheduler() {}
        public void ScheduleExamStructure()
        {
            GRBEnv env = new GRBEnv(true);
            env.Set("LogFile", "mip1.log");
            env.Start();
            model = new GRBModel(env);

            Structure.Variables.Set(model);
            Structure.Constraints.Set(model);
            Structure.Objective.Set(model);
            model.Optimize();
            if (model.Status == GRB.Status.OPTIMAL) {
                Structure.Reader.Get();
                model.Dispose();
                env.Dispose();
            } else {
                model.ComputeIIS();
                throw new StructureSchedulerException("The structure model proved to be infeasible.");
            }
        }

        public void ScheduleExamStructure(int numberOfDaysAvailableToConductTheExam)
        {
            GRBEnv env = new GRBEnv(true);
            env.Set("LogFile", "mip1.log");
            env.Start();
            model = new GRBModel(env);

            Structure.Variables.Set(model);
            Structure.Constraints.Set(model);
            Structure.Objective.Set(model);
            model.Optimize();
            Structure.Reader.Get();
        }
        public void ScheduleExamAttendants()
        {
            GRBEnv env = new GRBEnv(true);
            env.Set("LogFile", "mip1.log");
            env.Start();
            model = new GRBModel(env);

            ExamAttendants.Variables.Set(model);
            ExamAttendants.Constraints.Set(model);
            ExamAttendants.Objective.Set(model);
            model.Optimize();
            ExamAttendants.Reader.Get();
        }

        /// Visualizer for ScheduleExamStructure
        /// Can be used after the model.Optimize() call
        //int I = Variables.iGRB.GetLength(0);
        //int D = Variables.iGRB.GetLength(1);
        //int C = Variables.iGRB.GetLength(2);
        //GRBLinExpr temp = 0;
        //for (int i = 0; i < I; i++)
        //{
        //    for (int d = 0; d < D; d++)
        //    {
        //        for (int c = 0; c < C; c++)
        //        {
        //            Console.Write(Variables.iGRB[i, d, c].X);
        //            temp.AddTerm(1, Variables.iGRB[i, d, c]);
        //        }
        //        Console.Write("  ");
        //    }
        //    Console.Write("\n");
        //}
        //Console.WriteLine(temp.Value);
    }
}