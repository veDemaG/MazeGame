using UnityEngine;

public class Cell_genrating 
{
    private GameObject wall;
    private GameObject floor;
    private int mazeX;//width/column
    private int mazeY;//height/row
    private float wall_size;
    public cell[,] cells;

    public void set_data(int x,int y,GameObject w,GameObject f,float size)
    {
        mazeX = x;
        mazeY = y;
        wall = w;
        floor = f;
        wall_size = size;
    }

    public void genrate_Cell()
    {
        //defining maze length and width
        cells = new cell[mazeY, mazeX];
        GameObject gs = new GameObject("Maze");
        for (int r = 0; r < mazeY; r++)
        {
            for (int c = 0; c < mazeX; c++)
            {
                cells[r, c] = new cell();
                GameObject cell = new GameObject("cell[" + r + "," + c + "]");

                //For now, use the same wall object for the floor!
                cells[r, c].floor = GameObject.Instantiate(floor, new Vector3(c * wall_size, -(wall_size / 2f), r * wall_size), Quaternion.identity) as GameObject;
               cells[r, c].floor.name = "Floor " + r + "," + c;
                cells[r, c].floor.transform.parent = cell.transform;

                if (c == 0)
                {
                    cells[r, c].left = GameObject.Instantiate(wall, new Vector3(((c * wall_size)- (wall_size / 2)), 0, r * wall_size), Quaternion.identity) as GameObject;
                    cells[r, c].left.name = "West Wall " + r + "," + c;
                    cells[r, c].left.transform.parent = cell.transform;

                }

                cells[r, c].right = GameObject.Instantiate(wall, new Vector3((wall_size / 2)+(c * wall_size), 0, r * wall_size), Quaternion.identity) as GameObject;
                cells[r, c].right.name = "East Wall " + r + "," + c;
                cells[r, c].right.transform.parent = cell.transform;

                if (r == 0)
                {
                    cells[r, c].top = GameObject.Instantiate(wall, new Vector3(c * wall_size, 0, ((r * wall_size)- (wall_size / 2))), Quaternion.identity) as GameObject;
                    cells[r, c].top.name = "North Wall " + r + "," + c;
                    cells[r, c].top.transform.Rotate(Vector3.up * 90f);
                    cells[r, c].top.transform.parent = cell.transform;

                }

                cells[r, c].bottom = GameObject.Instantiate(wall, new Vector3(c * wall_size, 0, ((wall_size / 2) + (r * wall_size))), Quaternion.identity) as GameObject;
                cells[r, c].bottom.name = "South Wall " + r + "," + c;
                cells[r, c].bottom.transform.Rotate(Vector3.up * 90f);
                cells[r, c].bottom.transform.parent = cell.transform;


                cell.transform.parent = gs.transform;

            }
        }
        set_data();

        Maze_making mm = new Maze_making(cells);
        mm.Making_maze();
    }

    private void set_data()
    {
        int random_span = Random.Range(0, mazeX - 1);
        int random_finish = Random.Range(0, mazeX - 1);
        cells[0,random_span].floor.gameObject.GetComponent<Renderer>().material.color = Color.red;
        cells[mazeY - 1,random_finish].floor.gameObject.GetComponent<Renderer>().material.color = Color.green;

        Game_Manager.instance.set_span_point(cells[0, random_span].floor.gameObject.transform.position);
        Game_Manager.instance.set_fnish_point(cells[mazeY - 1, random_finish].floor.gameObject.transform.position);

        cells[mazeY - 1, random_finish].floor.gameObject.tag = "Finish";
        cells[0, random_span].floor.gameObject.tag = "spawn";
    }
}
