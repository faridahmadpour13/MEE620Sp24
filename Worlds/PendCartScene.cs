//============================================================================
// PendCartScene.cs
// Code for runing the Godot scene of a planar planar pendulum attached
// a translating cart.
//============================================================================
using Godot;
using System;

public partial class PendCartScene : Node3D
{
	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	// model stuff
	PendCartModel model;

	// data display stuff
	// Data display stuff
	UIPanelDisplay datDisplay;
	int uiRefreshCtr;     //counter for display refresh
	int uiRefreshTHold;   // threshold for display refresh

	// Mode of operation
	enum OpMode
	{
		SetPosition,
		SetAngle,
		Simulate,
	}

	OpMode opMode;         // operation mode
	float cartX;           // position of cart
	float pendAngle;       // angle of pendulum
	float dxMan;       // amount cart position is changed manually
	float dthetaMan;   // amount angle is changed each time updated manually
	bool manChanged;   // position or angle has been changed manually

	Label instructLabel;   // label to display instructions
	String instManPos;     // instructions when in SetPosition mode
	String instManAngle;   // instructions when in SetAngle mode
	String instSim;        // instructions when in Simulate model

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("Pendulum Cart Scene");

		// build the simulation
		float wallHeight = 2.0f;
		double pendLength = 1.5;
		//double pendMass = 1.9;
		//double cartMass = 2.8;
		opMode = OpMode.SetPosition;
		dxMan = 0.02f;
		dthetaMan = 0.03f;
		manChanged = false;

		// build the model
		model = GetNode<PendCartModel>("PendCartModel");
		model.Position = new Vector3(0.0f, wallHeight, 0.0f);
		model.PendulumLength = (float)pendLength;

		// Set up the camera rig
		longitudeDeg = 30.0f;
		latitudeDeg = 15.0f;
		camDist = 4.0f;
		camFOV = 55.0f;

		camTg = new Vector3(0.0f, wallHeight, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.FOVDeg = camFOV;
		cam.Target = camTg;

		// Set up data display
		datDisplay = GetNode<UIPanelDisplay>(
			"UINode/MarginContainer/DatDisplay");
		datDisplay.SetNDisplay(8);
		datDisplay.SetLabel(0,"Mode");
		datDisplay.SetValue(0,opMode.ToString());
		datDisplay.SetLabel(1,"Cart Pos.");
		datDisplay.SetValue(1, 0.0f);
		datDisplay.SetLabel(2,"Angle (deg)");
		datDisplay.SetValue(2, 0.0f);
		datDisplay.SetLabel(3,"Kinetic");
		datDisplay.SetValue(3,"---");
		datDisplay.SetLabel(4,"Potential");
		datDisplay.SetValue(4,"---");
		datDisplay.SetLabel(5,"Tot Energy");
		datDisplay.SetValue(5,"---");
		datDisplay.SetLabel(6,"Mass Ctr X");
		datDisplay.SetValue(6,"---");
		datDisplay.SetLabel(7,"Mass Ctr Y");
		datDisplay.SetValue(7,"---");

		datDisplay.SetDigitsAfterDecimal(1,2);
		datDisplay.SetDigitsAfterDecimal(2,1);
		datDisplay.SetDigitsAfterDecimal(3,4);
		datDisplay.SetDigitsAfterDecimal(4,4);
		datDisplay.SetDigitsAfterDecimal(5,4);
		datDisplay.SetDigitsAfterDecimal(6,4);
		datDisplay.SetDigitsAfterDecimal(7,4);

		uiRefreshCtr = 0;
		uiRefreshTHold = 3;

		instructLabel = GetNode<Label>(
			"UINode/MarginContainerBL/InstructLabel");
		instManPos = "Press left & right arrows to change cart position; " +
			"<Tab> to change pendulum angle; <Space> to simulate.";
		instManAngle = "Press left & right arrows to change pendulum angle; "+
			"<Tab> to change cart position; <Space> to simulate.";
		instSim = "Press <Space> to stop simulation and change " +
			"initial conditions.";
		instructLabel.Text = instManPos;
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{

		if(opMode == OpMode.SetPosition){
			if(Input.IsActionPressed("ui_right")){
				cartX += dxMan;
				model.SetPositionAngle(cartX, pendAngle);
				manChanged = true;
				datDisplay.SetValue(1, cartX);
				datDisplay.SetValue(3, "---");
				datDisplay.SetValue(4, "---");
				datDisplay.SetValue(5, "---");
			}

			if(Input.IsActionPressed("ui_left")){
				cartX -= dxMan;
				model.SetPositionAngle(cartX, pendAngle);
				manChanged = true;
				datDisplay.SetValue(1, cartX);
				datDisplay.SetValue(3, "---");
				datDisplay.SetValue(4, "---");
				datDisplay.SetValue(5, "---");
			}

			if(Input.IsActionJustPressed("ui_focus_next")){
				opMode = OpMode.SetAngle;
				datDisplay.SetValue(0, opMode.ToString());
				instructLabel.Text = instManAngle;
			}

			if(Input.IsActionJustPressed("ui_accept")){
				if(manChanged){
					// sim.Angle1 = (double)pend1Rotation.Z;
					// sim.Angle2 = (double)pend2Rotation.Z;
					// sim.GenSpeed1 = 0.0;
					// sim.GenSpeed2 = 0.0;
				}

				opMode = OpMode.Simulate;
				datDisplay.SetValue(0, opMode.ToString());
				instructLabel.Text = instSim;
				manChanged = false;
			}
			return;
		} // if opMode.SetPosition

		if(opMode == OpMode.SetAngle) {
			if(Input.IsActionPressed("ui_right")){
				pendAngle += dthetaMan;
				model.SetPositionAngle(cartX, pendAngle);
				manChanged = true;
				datDisplay.SetValue(2, Mathf.RadToDeg(pendAngle));
				datDisplay.SetValue(3, "---");
				datDisplay.SetValue(4, "---");
				datDisplay.SetValue(5, "---");
			}

			if(Input.IsActionPressed("ui_left")){
				pendAngle -= dthetaMan;
				model.SetPositionAngle(cartX, pendAngle);
				manChanged = true;
				datDisplay.SetValue(2, Mathf.RadToDeg(pendAngle));
				datDisplay.SetValue(3, "---");
				datDisplay.SetValue(4, "---");
				datDisplay.SetValue(5, "---");
			}

			if(Input.IsActionJustPressed("ui_focus_next")){
				opMode = OpMode.SetPosition;
				datDisplay.SetValue(0, opMode.ToString());
				instructLabel.Text = instManPos;
			}

			if(Input.IsActionJustPressed("ui_accept")){
				if(manChanged){
					// sim.Angle1 = (double)pend1Rotation.Z;
					// sim.Angle2 = (double)pend2Rotation.Z;
					// sim.GenSpeed1 = 0.0;
					// sim.GenSpeed2 = 0.0;
				}

				opMode = OpMode.Simulate;
				datDisplay.SetValue(0, opMode.ToString());
				instructLabel.Text = instSim;
				manChanged = false;
			}
			return;
		} // if OpMode.SetAngle
	}
}