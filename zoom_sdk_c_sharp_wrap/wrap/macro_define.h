#pragma once

#define BEGIN_CLASS_DEFINE(Classname) \
class CSDKWrap;\
class CSDKExtWrap;\
class Classname##Wrap \
{

#define BEGIN_CLASS_DEFINE_WITHCALLBACK(Classname, CallBackClass)\
class CSDKWrap;\
class CSDKExtWrap;\
class Classname##Wrap : public CallBackClass \
{

#define STAITC_CLASS(Classname) \
friend CSDKWrap;\
friend CSDKExtWrap;\
private:\
	Classname##Wrap(){m_obj = NULL;};\
	static Classname##Wrap& GetInst(){static Classname##Wrap inst; return inst;};\
public:

#define NORMAL_CLASS(Classname) \
public:\
	Classname##Wrap(){m_obj = NULL;};

#define INIT_UNINIT(Classname)\
	void Init(){m_obj = Init##Classname##Func();};\
	void Uninit(){Uninit##Classname##Func(m_obj);m_obj=NULL;};

#define INIT_UNINIT_WITHEVENT(Classname)\
	void Init(){m_obj = Init##Classname##Func(this);};\
	void Uninit(){Uninit##Classname##Func(m_obj);m_obj=NULL;};

#define INIT_UNINIT_WITHEVENT_AND_OWNSERVICE(Classname, OwnerClass)\
	void Init(OwnerClass* pOwner){m_obj = Init##Classname##Func(this, pOwner);};\
	void Uninit(){Uninit##Classname##Func(m_obj);m_obj=NULL;};

#define DEFINE_FUNC_0(funcname,R)\
	R funcname();

#define DEFINE_FUNC_1(funcname,R,T1,P1)\
	R funcname(T1 P1);

#define DEFINE_FUNC_2(funcname,R,T1,P1,T2,P2)\
	R funcname(T1 P1, T2 P2);

#define DEFINE_FUNC_3(funcname,R,T1,P1,T2,P2,T3,P3)\
	R funcname(T1 P1, T2 P2, T3 P3);

#define DEFINE_FUNC_4(funcname,R,T1,P1,T2,P2,T3,P3,T4,P4)\
	R funcname(T1 P1, T2 P2, T3 P3, T4 P4);

#define DEFINE_FUNC_AND_MEMBER(funcname,R)\
 	R& funcname();\
 	private:\
 	R m_ob##R;\
 	public:

#define CallBack_FUNC_0(funcname)\
virtual void funcname()\
{\
	if (NULL != m_cb##funcname)\
		m_cb##funcname();\
}\
std::function<void()> m_cb##funcname;

#define CallBack_FUNC_1(funcname, T1, P1)\
virtual void funcname(T1 P1)\
{\
	if (NULL != m_cb##funcname)\
		m_cb##funcname(P1);\
}\
std::function<void(T1)> m_cb##funcname;

#define CallBack_FUNC_2(funcname, T1, P1, T2, P2)\
virtual void funcname(T1 P1, T2 P2)\
{\
	if (NULL != m_cb##funcname)\
		m_cb##funcname(P1, P2);\
}\
std::function<void(T1, T2)> m_cb##funcname;

#define CallBack_FUNC_3(funcname, T1, P1, T2, P2, T3, P3)\
virtual void funcname(T1 P1, T2 P2, T3 P3)\
{\
	if (NULL != m_cb##funcname)\
		m_cb##funcname(P1, P2, P3);\
}\
std::function<void(T1, T2, T3)> m_cb##funcname;

#define END_CLASS_DEFINE(Classname) \
public:\
	Classname* GetSDKObj(){return m_obj;};\
private:\
	Classname* m_obj;\
};

#define IMPL_FUNC_0(Classname,funcname,R,DEF_RET)\
	R Classname##Wrap::funcname()\
{\
	if (m_obj)\
	return m_obj->funcname();\
	return DEF_RET;\
}

#define IMPL_FUNC_1(Classname,funcname,R,T1,P1,DEF_RET)\
R Classname##Wrap::funcname(T1 P1)\
{\
if (m_obj)\
return m_obj->funcname(P1);\
return DEF_RET;\
}

#define IMPL_FUNC_2(Classname,funcname,R,T1,P1,T2,P2,DEF_RET)\
R Classname##Wrap::funcname(T1 P1, T2 P2)\
{\
if (m_obj)\
return m_obj->funcname(P1, P2);\
return DEF_RET;\
}

#define IMPL_FUNC_3(Classname,funcname,R,T1,P1,T2,P2,T3,P3,DEF_RET)\
R Classname##Wrap::funcname(T1 P1, T2 P2, T3 P3)\
{\
if (m_obj)\
return m_obj->funcname(P1, P2, P3);\
return DEF_RET;\
}

#define IMPL_FUNC_4(Classname,funcname,R,T1,P1,T2,P2,T3,P3,T4,P4,DEF_RET)\
R Classname##Wrap::funcname(T1 P1, T2 P2, T3 P3, T4 P4)\
{\
if (m_obj)\
return m_obj->funcname(P1, P2, P3, P4);\
return DEF_RET;\
}

#define IMPL_FUNC_NORET_0(Classname,funcname,R)\
	R Classname##Wrap::funcname()\
{\
if (m_obj)\
m_obj->funcname();\
}

#define IMPL_FUNC_NORET_1(Classname,funcname,R,T1,P1)\
R Classname##Wrap::funcname(T1 P1)\
{\
if (m_obj)\
m_obj->funcname(P1);\
}

#define IMPL_FUNC_NORET_2(Classname,funcname,R,T1,P1,T2,P2)\
R Classname##Wrap::funcname(T1 P1, T2 P2)\
{\
if (m_obj)\
m_obj->funcname(P1, P2);\
}

#define IMPL_FUNC_NORET_3(Classname,funcname,R,T1,P1,T2,P2,T3,P3)\
R Classname##Wrap::funcname(T1 P1, T2 P2, T3 P3)\
{\
if (m_obj)\
m_obj->funcname(P1, P2, P3);\
}

#define IMPL_FUNC_NORET_4(Classname,funcname,R,T1,P1,T2,P2,T3,P3,T4,P4)\
R Classname##Wrap::funcname(T1 P1, T2 P2, T3 P3, T4 P4)\
{\
if (m_obj)\
m_obj->funcname(P1, P2, P3, P4);\
}


#define IMPL_FUNC_AND_MEMBER(Classname,funcname,R)\
R##& Classname##Wrap::funcname()\
{\
if (m_obj)\
m_ob##R.Init(this);\
return m_ob##R;\
}