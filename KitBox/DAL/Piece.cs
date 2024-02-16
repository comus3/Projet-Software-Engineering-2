
namespace DAL ; 


public class Piece : Model
{

    public Piece(Connection connection) : base(connection)
    {
        tableName = "piece";
        primaryKey = "code";
    }
}