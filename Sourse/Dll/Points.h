#pragma once // ����� �� ������� ���������
#include <cstdlib> // ��� ������������� ��������� ���. �������

// ----------- ��������� - ����������  ---------------------------
struct Point
{
	int X, Y; // ���������� ������ 
	Point(){X = -1; Y = -1;}; // ����������� �� ���������
	Point(int newX, int newY){X = newX; Y = newY;}; // ����������� � ������������
	Point& Set(int newX, int newY){X = newX; Y = newY; return *this;}; // ���������� ���������� ������
	bool operator==(const Point& Pt){return ((X == Pt.X) && (Y == Pt.Y));}; // �������� "�����"
	bool operator!=(const Point& Pt){return ((X != Pt.X) || (Y != Pt.Y));}; // �������� "�� �����"
	Point& operator>>(int n){X += abs(n); Y += n; return *this;}; //�������� "�������� ���������� ������"
	Point& operator<<(int n){X -= abs(n); Y += n; return *this;}; //�������� "�������� ���������� �����" 
}; // - Point

// -----------  ����� - ������ ���������  ------------------
class Points
{	
	struct PList //������� ������
	{
		Point Pt; PList* pNext; // ������� - ����������, ����� ���������� ��������
		PList(PList* pPrv, const Point& P) : Pt(P){pNext = 0; if(pPrv) pPrv -> pNext = this;};  // ����������� � ���������� ���������� �
	}; // - PList
				// -----------------------------------------
	int nCount; PList* pFirst; PList* pLast; PList* pCur; // ���������� ���������, ������, ��������� � ������� �������� ������ 
				// -----------------------------------------
public:
	Points(){nCount = 0; pFirst = pLast = pCur = 0;}; // ����������� �� ���������
				// -----------------------------------------
	void Clear() // ��������� ������
	{
		if(!nCount) return; PList* pL = pFirst; // ���� ������ ���� - �������... ����� �������� ��������
		for(int i = 0; i < nCount; i++){PList* pDlt = pL; pL = pL -> pNext; delete pDlt;} // ������� ���������� ��������
		nCount = 0; pCur = pFirst = pLast = 0; // �������� ���������
	}; // - Clear
				// -----------------------------------------
	void Add(const Point& Pt) // �������� � ����� ������� ���������� Pt
	{
		PList* pL = new PList(pLast, Pt); pCur = pLast = pL; // ������� ������� �� ���������� �������� � ������������� � �����. ������������� ���������.
		if(!nCount) pFirst = pL; nCount++; // ���� ������� ������, ������������� ���������. ����������� �������.
	}; // - Add
				// -----------------------------------------
	void Addl(Points& Pts) // �������� � ����� ������� ���������� �� ������ Pts
	{
		if(!Pts.ToStart()) return; // ���� ����� ������ ���� - �������.
		Point Ptt; while(Pts.Next(Ptt)) Add(Ptt); // ��������� ���������� �������� ������ ������ � ����� �������.
	}; // - Addl
				// -----------------------------------------
	Points& operator=(Points& Pts) //�������� ������������
	{
		if(this == &Pts) return *this; // ���� ������ �� ����������� ������ - �������
		Clear(); Addl(Pts);  return *this; // ������� ������, ��������� � ����� ����� ������, ���������� ������� ������.
	}; // - operator=
				// -----------------------------------------
	int Count(){return nCount;}; // ������� ���������� ��������� ������ 
	bool ToStart(){if(!pFirst) return false; pCur = pFirst; return true;}; // ������� � ������� �������� ������
	bool Last(Point& P){if(!pLast) return false; P = pLast -> Pt; return true;}; // �������� ��������� ������� ������
	bool First(Point& P){if(!pFirst) return false; P = pFirst -> Pt; return true;}; // �������� ������ ������� ������
	bool Next(Point& P){if(!pCur) return false; P = pCur -> Pt; pCur = pCur -> pNext; return true;};// �������� ��������� ������� ������
	~Points(){Clear();}; // ���������� (� �������� ������)
}; // - Points
// -----------------------------------------