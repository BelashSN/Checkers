#include "stdafx.h" // для dll
#include "Game.h" // Класс игры

// ---- Объявление глобальных переменных программы -----------------
CBuffer Buffer; // Буфер обмена 
Game game; // Текущая Игра 
Point SFPt[2];

// ---------- * ---------------- * --------------- * ------------
// ---------- * ---------------- * --------------- * ------------
// ---------- * ---------------- * --------------- * ------------
void ReText(int*& n, Move Mv)
{
	Point Pt; Mv.Pts.First(Pt); n[66] = Pt.X; n[67] = Pt.Y; n[68] = 32;  
	if(Mv.Kill) n[69] = 58; else n[69] = 45; 
	Mv.Pts.Last(Pt); n[70] = 32; n[71] = Pt.X; n[72] = Pt.Y;
}

// ---------- * ---------------- 
void ReField(int*& n)
{
	for(int j = 0; j < 8; j++) for(int i = 0; i < 8; i++) 
		n[i + j * 8] = static_cast<int>(Buffer.Field[i][j]);
	n[64] = static_cast<int>(game.GRotate());
}

// ---------- * ---------------- 
void SetErrors(int*& n)
{
	if(!Buffer.Mvs.ToStart()) return; Point Pt; Move Mv;
	while(Buffer.Mvs.Next(Mv))
	{	
		if(!Mv.Pts.ToStart()) continue; char Fg;
		while (Mv.Pts.Next(Pt)) if(n[Pt.Y + Pt.X * 8] < 30) n[Pt.Y + Pt.X * 8] += 50; 
		Mv.Pts.First(Pt); if(n[Pt.Y + Pt.X * 8] < 80) n[Pt.Y + Pt.X * 8] += 50;
		Mv.Pts.Last(Pt); if(n[Pt.Y + Pt.X * 8] < 80) n[Pt.Y + Pt.X * 8] += 50;
	} 
}
// ---------- * ---------------- * --------------- * ------------
// ---------- * ---------------- * --------------- * ------------
// ---------- * ---------------- * --------------- * ------------
extern "C"
{
	__declspec(dllexport) void LibManualMove(int*& n, int X1, int Y1, int X2, int Y2)
	{
		
		n  = new int[75]; SFPt[0].Set(X1, Y1); SFPt[1].Set(X2, Y2);
		int Res = game.ManualMove(SFPt); ReField(n); n[65] = Res;
		switch(Res)
		{
			case reOk && reGameOver: {Move Mv; Buffer.Mvs.First(Mv); ReText(n, Mv); break;}
			default: SetErrors(n);
		}

	}

	// ---------- * ---------------- 
	__declspec(dllexport) void LibSetField(int*& n)
		{
			char TF[8][8];
			for(int j = 0; j < 8; j++) for(int i = 0; i < 8; i++) TF[i][j] = static_cast<char>(n[i + j * 8]);
			game.SField(TF);
		}

	// ---------- * ---------------- 
	__declspec(dllexport) void LibAutoMove(int*& n)
	{
		n  = new int[75]; int Res = game.AutoMove(); ReField(n); n[65] = Res;
		switch(Res)
		{
			case reOk && reGameOver: {Move Mv; Buffer.Mvs.First(Mv); ReText(n, Mv); break;}
		}
	}

	// ---------- * ---------------- 
	__declspec(dllexport) void LibNew(int*& n) {n  = new int[75]; game.New(); ReField(n);}

	// ---------- * ---------------- 
	__declspec(dllexport) void LibSetMaster(int W, int B) {game.SMaster(W == 1, B == 1);}

	// ---------- * ---------------- 
	__declspec(dllexport) void LibSetLevel(int L) {game.SBuf(L);}

	// ---------- * ---------------- 
	__declspec(dllexport) int LibReMaster() {if(game.ReMaster(false)) return 1; else return 0;}

	// ---------- * ---------------- 

}

// ---------- * ---------------- * --------------- * ------------
// ---------- * ---------------- * --------------- * ------------
BOOL APIENTRY DllMain (HANDLE hModule, DWORD reason, LPVOID lpReserved)
{
	switch (reason)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:	break;
	}
    return TRUE;
}
