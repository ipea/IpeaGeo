using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IpeaGeo.Modelagem.RotaOtima
{
    public class clsDistanciaMinima
    {

    }

    public class LinhaFromShape
    {
        public LinhaFromShape(string tipo_modal, double dist_in_shape, string vertice_id)
        {
            IDinShape = vertice_id;
            DistTotalLinhaInShape = dist_in_shape;
            TipoModal = tipo_modal;
        }

        public double DistTotalCalculada { get; set; }
        public double DistTotalLinhaInShape { get; set; }
        public string IDinShape { get; set; }
        public string TipoModal { get; set; } // hidrovia, ferrovia, rodovia
    }

    public class Vertice
    {
        public Vertice(int node_ID1, int node_ID2, string tipo, double distancia)
        {
            NodeID1 = node_ID1;
            NodeID2 = node_ID2;
            Distance = distancia;
            tipo_modal = tipo;
            CustoUnitario = 0.0;
            CustoTotal = 0.0;
        }

        public double DistTotalCalculada { get; set; }
        public double DistTotalLinhaInShape { get; set; }
        public string VerticeIDinShape { get; set; }
        public int NodeID1 { get; set; }
        public int NodeID2 { get; set; }
        public double Distance { get; set; }
        public string tipo_modal { get; set; } // hidrovia, ferrovia, rodovia
        public double CustoUnitario { get; set; }
        public double CustoTotal { get; set; }
        public double DistanceOld { get; set; }
        public int SequencialArquivosShape { get; set; } 
    }

    public class Node
    {
        public Node(double x, double y, int id, string tipo_modal)
        {
            visited = false;
            X = x;
            Y = y;
            node_ID = id;

            ConectedNodes = new ArrayList();
            DistanceToNodes = new ArrayList();
            TipoConexoesToNodes = new ArrayList();
            TipoModalNo = tipo_modal;
        }

        public int node_ID { get; set; } 
        public double X { get; set; }
        public double Y { get; set; }
        public ArrayList ConectedNodes { get; set; }
        public ArrayList DistanceToNodes { get; set; }
        public ArrayList TipoConexoesToNodes { get; set; }
        public bool visited { get; set; }
        public string TipoModalNo { get; set; }
    }
}
