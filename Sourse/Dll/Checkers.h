#pragma once //���� ���������� ����������
#include "Moves.h" // ����� �� ������� ����� 

//----- ������������ ----------------------------------------------------------------
enum eColor {colWhite, colBlack}; //�����
enum eLevel {llow = 3, lmedium = 4, lhigh = 5}; //������ ���� (� ������������ �������� �������� � ���������)
enum eResults {reOk, reBed, reKill, reGameOver, reDrawGame}; // reNot, reAlien, reBlock, ���������� ��������� ������
//enum eCommands {cmExit,  cmRotate, cmComputer, cmNew, cmMove, cmLevel, cmError}; // �������

//----- ���������� ������� / ��������� ----------------------------------------------
		
inline bool fFigure(const char F){return abs(F) > 1;}; //���������� ������� / ���������� ������ (true = � �������) �� �������� ����
inline bool fFColor(const char F){return (F >> 8) * -1;}; //���������� ���� ������ (false = �����) �� �������� ����
inline void pSField(char (*DF)[8], const char (*SF)[8]) //�������� �������� ���� ���� �� ��������� SF � �������� DF
	{for(int j = 0; j < 8; j++) for(int i = 0; i < 8; i++) DF[i][j] = SF[i][j];}; // � ������ ������ ���� - �������� ��������.
inline int fRotating(const int i, const bool R){return i * !R + (7 - i) * R;}; //���������� ������ �������� ���� i, ����������� ��������� (��� R == true) �� ��������: 8 = 1, 7 = 2, � ��.

//----- ��������� - ����� ������ (��������� ��������� ������)-------------------------
struct CBuffer
{
	Moves Mvs; //������ �����
	char Field[8][8]; // ���� ���� (8�8)
	bool Master; //���� ������ �������� ����
	void Add(Move& Mv){	Mvs.Add(Mv);}; //�������� ��� � ������
	void Set(char (*SF)[8]){pSField(Field, SF);}; //���������� �������� ���� ���� Field �� ��������� SF
	void Set(char (*SF)[8], bool Mst){Set(SF);  Master = Mst;};//���������� �������� ���� ���� �� SF � ���� ������ �������� ����
	void Addl(Moves& SMvs) //���������� (��������) ������ ����� �� ��������� SMvs
	{
		if(!SMvs.ToStart()) return; //���� SMvs ���� (��� ������� ��������) - �������
		Mvs.Clear(); Move Mv; //������� ������ � ������� ���������� ���� ���
		while(SMvs.Next(Mv)) Mvs.Add(Mv); //���� ���� ��������� ������� SMvs, ��������� ��� � ������
	}; // - Addl
}; // -CBuffer