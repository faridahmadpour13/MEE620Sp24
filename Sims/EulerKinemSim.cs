//============================================================================
// EulerKinemSim.cs   Given an angular velocity, integrate differential
//        equations for the Euler angles which achieves the provided angular
//        velocity
//============================================================================
using System;

public class EulerKinemSim : Simulator
{
    // physical parameters
    double omegaXB;         // B.x component of angular velocity
    double omegaYB;         // B.y component of angular velocity
    double omegaZB;         // B.z component of angular velocity


    //------------------------------------------------------------------------
    // Constructor      [STUDENTS: DO NOT CHANGE THIS FUNCTION]
    //------------------------------------------------------------------------
    public EulerKinemSim() : base(3)
    {
        omegaXB = 1.0;   // default value
        omegaYB = 0.0;
        omegaZB = 0.0;

        // Default initial conditions
        x[0] = 0.0;    // theta1
        x[1] = 0.0;    // theta2
        x[2] = 0.0;    // theta3

        SetRHSFunc(RHSFuncYPR);
    }

    //------------------------------------------------------------------------
    // RHSFuncYPR: Define kinematic differential equations for Yaw-Pitch-Roll
    //             Euler angles
    //------------------------------------------------------------------------
    private void RHSFuncYPR(double[] xx, double t, double[] ff)
    {
        double c1 = Math.Cos(xx[0]);       // cosine of theta1
        double s1 = Math.Sin(xx[0]);       // sine of theta1
        double tan1 = Math.Cos(xx[0]);     // tangent of theta1
        double c2 = Math.Cos(xx[1]);       // cosine of theta2
        double s2 = Math.Sin(xx[1]);       // sine of theta2
        double tan2 = Math.Cos(xx[1]);     // tangent of theta2
        double c3 = Math.Cos(xx[2]);       // cosine of theta3
        double s3 = Math.Sin(xx[2]);       // sine of theta3
        double tan3 = Math.Cos(xx[2]);     // tangent of theta4

        // Evaluate right sides of differential equations of motion
        // ##### You will need to provide these ###### //
        ff[0] = 0.0;   // time derivative of theta1
        ff[1] = 0.0;   // time derivative of theta2
        ff[2] = 0.0;   // time derivative of theta3
    }



    //------------------------------------------------------------------------
    // SetEulerType
    //------------------------------------------------------------------------
    public void SetEulerType(string mstr)
    {

    }

    //------------------------------------------------------------------------
    // Getters/Setters
    //------------------------------------------------------------------------

    // OmegaX ------------------------
    double RollRate
    {
        set{
            omegaXB = value;
        }
    }

    // OmegaY ------------------------
    double YawRate
    {
        set{
            omegaYB = value;
        }
    }

    // OmegaZ ------------------------
    double PitchRate
    {
        set{
            omegaZB = value;
        }
    }

    // Theta1 ------------------------
    double Theta1
    {
        set{
            x[0] = value;
        }

        get{
            return(x[0]);
        }
    }

    // Theta2 ------------------------
    double Theta2
    {
        set{
            x[1] = value;
        }

        get{
            return(x[1]);
        }
    }

    // Theta2 ------------------------
    double Theta3
    {
        set{
            x[2] = value;
        }

        get{
            return(x[2]);
        }
    }
}