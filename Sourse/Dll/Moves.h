#pragma once // ����� �� ������� �����
#include "Points.h" // ����������

// --------- ��������� - ��� (�� ������� ���������) -----------------------
struct Move
{
	int Weight, OppCount; // ��� ����, ���������� ��������� ����� ���������� ����� ����
	bool Kill; Points Pts; // ���� �������� ����, ������ �����������
	Move(){}; // ����������� �� ���������
	Move(const Point& Pt) {Kill = OppCount = Weight = 0; Pts.Add(Pt);}; // ����������� � ��������� ����������� ������
	Move(Move& M){Kill = M.Kill; OppCount = M.OppCount; Weight = M.Weight; Pts = M.Pts;}; // ����������� �����������
}; // - Move

// ----------- ����� - ������ ����� ---------------------------
class Moves
{
	struct MList // ������� ������
	{
		Move Mv; MList* pPrev; MList* pNext; // ������� - ���, ����� �����������, ����� ���������� ���������
		MList(MList* pPrv, Move& M) : Mv(M){pNext = 0; pPrev = pPrv; if(pPrv) pPrv -> pNext = this;}; // ����������� � ���������� ���� �
	};// - MList
				// -----------------------------------------
	int nCount; MList* pFirst; MList* pLast; MList* pCur; MList* pDel; // ���������� ���������, ������, ���������, ������� � ��������� �������� ������
	void ReSet(Move& DM, Move& SM) // ���������� ��� �� ��������� SM � �������� DM 
		{DM.OppCount = SM.OppCount; DM.Weight = SM.Weight; DM.Kill = SM.Kill; DM.Pts = SM.Pts;}; // �������� ���� �������
				// -----------------------------------------
public:
	bool Kills; // ���� �� � ������ ������� ���� (��� ����������)
	Moves(){Kills = nCount = 0; pFirst = pLast = pCur = 0;}; // ����������� �� ���������
				// -----------------------------------------
	void Clear() // ��������� ������
	{
		if(!nCount) return; MList* mL = pFirst; // ���� ������ ���� - �������... ����� �������� ��������
		for(int i = 0; i < nCount; i++){MList* pDlt = mL; mL = mL -> pNext; delete pDlt;} // ������� ���������� ��������
		Kills = nCount = 0; pDel = pCur = pFirst = pLast = 0; // �������� ���������
	}; // - Clear
				// -----------------------------------------
	void Add(Move& Mv) // �������� � ����� ������� ��� Mv
	{
		MList* mL = new MList(pLast, Mv); pCur = pLast = mL;  // ������� ������� �� ���������� �������� � ������������� � �����. ������������� ���������.
		if(!nCount) pFirst = mL; pDel = 0; nCount++; // ���� ������� ������, ������������� ���������. �������� ��������� �� ��������� �������. ����������� �������.
	}; // - Add
				// -----------------------------------------
	bool Delete() // ������� ������� ������ (pDel)
	{
		if(!pDel) return false; // ���� �� ���������� ��������� - ������� ����.
		if(pDel == pFirst) pFirst = pDel -> pNext; // ���� ��������� ������� ������ - ������ ���������
		if(pDel -> pNext) pDel -> pNext -> pPrev = pDel -> pPrev; // ���� �����, ������ ���������
		if(pDel -> pPrev) pDel -> pPrev -> pNext = pDel -> pNext; // �������� ���������
		delete pDel; nCount--; return true; // ������� �������, ��������� �������, ������� - ������
	}; // - Delete
				// -----------------------------------------
	void Sorted(int n) // ����������� ������ (�� �������� ���� ����) ������� � �������� n
	{
		MList* mL = pFirst; MList* cmL;  MList* tmL; // ������� �������, ���������� (������) � ������ ��������
		for(int m = 1; mL; m++) // ���� ��� m, ���� mL � �������� ������
		{
			if(m == n) cmL = tmL = mL; // ���� ����� �� �������� n, ������������� ���������� � ������ ��������
			if(m > n) // ���� ����� �������� ������ n
				if((cmL -> Mv.Weight < mL -> Mv.Weight) || //���� ��� ���������� ���� ������ ���� ��������
				((cmL -> Mv.Weight == mL -> Mv.Weight) && (cmL -> Mv.OppCount > mL -> Mv.OppCount))) // ��� ���� �����, �� ���������� ����� ���������� ������
					cmL = mL; // ������������� ���������� ������� �� �������
			mL = mL -> pNext; // ��������� �������
		} // - for
		if(tmL == cmL) return; // ���� ���������� ������� ����� ������� - �������
		Move tMv(tmL -> Mv); ReSet(tmL -> Mv, cmL -> Mv); ReSet(cmL -> Mv, tMv); // ������� ����� ���� � �\� ��� ������ ������ � ���������� �������� �������
	}; // - Sorted
				// -----------------------------------------
	 int Count(){pDel = 0; return nCount;}; // ������� ���������� ��������� ������ 
	bool ToStart(){pDel = 0; if(!pFirst) return false; pCur = pFirst; return true;}; // ������� � ������� �������� ������
	bool First(Move& M){pDel = 0; if(!pFirst) return false; M = pFirst -> Mv; return true;}; // �������� ������ ������� ������
	void Set(Move& M){if(!pDel) return; pDel -> Mv.OppCount = M.OppCount; pDel -> Mv.Weight = M.Weight;}; // ���������� � ������� ������� ������ �������� �� ���� Mv
	bool Next(Move& M){pDel = pCur; if(!pCur) return false; M = pCur -> Mv; pCur = pCur -> pNext; return true;}; // �������� ��������� ������� ������
	~Moves(){Clear();}; //���������� (� �������� ������)
}; // - Moves
// -----------------------------------------