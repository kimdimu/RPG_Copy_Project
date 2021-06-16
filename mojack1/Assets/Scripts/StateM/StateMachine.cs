using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine<entity_type>
{
    entity_type m_pOwner;
    State<entity_type> m_pCS;
    State<entity_type> m_pPS;
    State<entity_type> m_pGS;

    private void Awake()
    {
        m_pOwner = default;
        m_pCS = null;
        m_pPS = null;
        m_pGS = null;
    }
    public void SetOwner(entity_type type)
    {
        m_pOwner = type;
    }
    public void SetCS(State<entity_type> state)
    {
        m_pCS = state;
    }
    public void SetPS(State<entity_type> state)
    {
        m_pPS = state;

    }
    public void SetGS(State<entity_type> state)
    {
        m_pGS = state;

    }

    public void sUpdate()
    {
        if (m_pGS != null)
        {
            m_pGS.Execute(m_pOwner);
        }
        if (m_pCS != null)
        {
            m_pCS.Execute(m_pOwner);
        }
    }

    public void ChangeState(State<entity_type> newstate)
    {
        m_pPS = m_pCS;
        m_pCS.Exit(m_pOwner);
        m_pCS = newstate;
        m_pCS.Enter(m_pOwner);
    }
    public void RevertToPreviousState()
    {
        ChangeState(m_pGS);
    }

    public State<entity_type> CurrentState() { return m_pCS; }
    public State<entity_type> GlobalState() { return m_pGS; }
    public State<entity_type> PreviousState() { return m_pPS; }

    public string GetNameOfCurrentState()
    {
        //std::string s(typeid(* m_pCurrentState).name());
        Type type = m_pCS.GetType();
        string s = type.ToString();

        //		if (s.size() > 5)
        //          s.erase(0, 6);
        if (s.Length > 5)
            s.Remove(0, 6);

        return s;
    }
}
