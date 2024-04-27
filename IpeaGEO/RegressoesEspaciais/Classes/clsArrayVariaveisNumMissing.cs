using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsArrayVariaveisNumMissing
    {
        public clsArrayVariaveisNumMissing()
        {
        }

        private ArrayList m_array_variaveis_missing = new ArrayList();

        public string[] NomesVariaveisMissing
        {
            get
            {
                string[] nomes = new string[this.m_array_variaveis_missing.Count];
                for (int i = 0; i < nomes.GetLength(0); i++)
                {
                    nomes[i] = ((clsVariaveisNumMissing)m_array_variaveis_missing[i]).NomeVariavelNum;
                }
                return nomes;
            }
        }

        public int Count
        {
            get
            {
                return m_array_variaveis_missing.Count;
            }
        }

        public void Add(clsVariaveisNumMissing v)
        {
            m_array_variaveis_missing.Add(v);
        }
        
        public clsVariaveisNumMissing this[int i]
        {
            get
            {
                if (i >= m_array_variaveis_missing.Count || i < 0)
                {
                    throw new Exception("Índice fora dos limites");
                }
                return (clsVariaveisNumMissing)m_array_variaveis_missing[i];
            }
            set
            {
                if (i >= m_array_variaveis_missing.Count || i < 0)
                {
                    throw new Exception("Índice fora dos limites");
                }
                m_array_variaveis_missing[i] = value;
            }
        }

        public clsVariaveisNumMissing this[string nome]
        {
            get
            {
                for (int i = 0; i < m_array_variaveis_missing.Count; i++)
                {
                    if (((clsVariaveisNumMissing)m_array_variaveis_missing[i]).NomeVariavelNum == nome)
                    {
                        return (clsVariaveisNumMissing)m_array_variaveis_missing[i];
                    }
                }
                return new clsVariaveisNumMissing();
            }
            set
            {
                for (int i = 0; i < m_array_variaveis_missing.Count; i++)
                {
                    if (((clsVariaveisNumMissing)m_array_variaveis_missing[i]).NomeVariavelNum == nome)
                    {
                        m_array_variaveis_missing[i] = value;
                    }
                }
            }
        }
    }
}
