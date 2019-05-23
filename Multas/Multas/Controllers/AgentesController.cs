using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multas.Models;

namespace Multas.Controllers
{
    public class AgentesController : Controller{

        //Criar Var que representa a BD
        private MultasDB db = new MultasDB();

        // GET: Agentes
        public ActionResult Index(){


            //procura a totalidade dos agentes na BD
            //Instrução feita em LINQ
            //SELECT * FROM Agentes ORDER BY nome
            var listaAgentes = db.Agentes.OrderBy(a => a.Nome).ToList();
            return View(listaAgentes);
        }




        // GET: Agentes/Details/5
        /// <summary>
        /// Mostra os dados de um Agente
        /// </summary>
        /// <param name="id">identifica o Agente</param>
        /// <returns>devolve a View com os dados</returns>
        public ActionResult Details(int? id){
            if (id == null){
                //vamos alterar esta resposta por defeito
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //
                //este erro ocorre porque o utilizador anda a fazer asneiras
                return RedirectToAction("Index");
            }
            // SELECT * FROM Agentes WHERE Id=id
            Agentes agentes = db.Agentes.Find(id);

            //O Agente foi encontrado?
            if (agentes == null)
            {

                //O Agente não foi encontrado, porque o utilizador está 'à pesca'
                //return HttpNotFound();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // GET: Agentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agentes/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// criação de um novo agente
        /// </summary>
        /// <param name="agentes">recolhe os dados do Nome e da Esquadra do Agente</param>
        /// <param name="fotografia">representa a fotografia que identifica o Agente</param>
        /// <returns>devolve uma view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Esquadra")] Agentes agentes, HttpPostedFileBase fotografia){

            /// precisamos de processar a fotografia
            /// 1º será que foi fornecido um ficheiro
            /// 2º será do tipo correto 
            /// 3º se for do tipo correto, guarda-se
            /// senão, atribui-se um ' avatar generico' ao utilizador

            //var auxiliar
            string caminho = "";
            bool haFicheiro = false;
            //há ficheiro?
            if (fotografia == null)
            {
                //não há ficheiro, atribui-se-lhe o avatar
                agentes.Fotografia = "nouser.png";
            }
            else
            {
                if(fotografia.ContentType =="image/jpeg" || fotografia.ContentType == "image/png")
                {
                    string extensao = Path.GetExtension(fotografia.FileName).ToLower();
                    Guid g;
                    g = Guid.NewGuid();
                    string nome = g.ToString() +extensao;

                    caminho = Path.Combine(Server.MapPath("~/imagens"),nome);
                    //atribuir ao agente o nome do ficheiro
                    agentes.Fotografia = nome;
                    //assinalar q ha foto
                    haFicheiro = true;
                }
            }

            if (ModelState.IsValid) //valida os dados fornecidos estão de acordo com as regras definidas no modelo 
            {
                try
                {
                    //adiciona o novo agente ao Modelo
                    db.Agentes.Add(agentes);
                    //consolida os dados na BD
                    db.SaveChanges();
                    //vou guardar o ficheiro no disco rigido
                    if(haFicheiro) fotografia.SaveAs(caminho);
                    //redireciona o utilizador para a página do INDEX
                    return RedirectToAction("Index");
                }
                catch(Exception)
                {
                    ModelState.AddModelError("", "Ocurreu um erro com a escrita dos dados do novo agente");
                }
                

                
            }

            return View(agentes);
        }

        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //vamos alterar esta resposta por defeito
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //
                //este erro ocorre porque o utilizador anda a fazer asneiras
                return RedirectToAction("Index");
            }
            // SELECT * FROM Agentes WHERE Id=id
            Agentes agentes = db.Agentes.Find(id);

            //O Agente foi encontrado?
            if (agentes == null)
            {

                //O Agente não foi encontrado, porque o utilizador está 'à pesca'
                //return HttpNotFound();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // POST: Agentes/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agentes){
            if (ModelState.IsValid)
            {
                db.Entry(agentes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // GET: Agentes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return HttpNotFound();
            }

            // O Agente foi encontrado 
            // vou salvaguardar os dados para posterior validação
            // - guardar o ID do Agente num Cookie cifrado
            // - guardar o ID numa variavel de sessão (se se usar o ASP . NET Core, esta ferramenta já não existe...)
            // - outras alternativas válidas...
            Session["Agente"] = agentes.ID;
            //mostra na View os dados do Agente
            return View(agentes);
        }

        // POST: Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {

            if(id == null)
            {
                //há um 'xico esperto' a tentar dar-me a volta ao código
                return RedirectToAction("Index");
            }

            //o ID não é null
            //será o ID o que eu espero?
            // vamos validar se o ID está correto
            if (id != (int)Session["Agente"])
            {
                // há aqui outro espertinho...
                return RedirectToAction("Index");
            }

            //procura o agente a remover
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                //nao foi encontrado o agente
                return RedirectToAction("Index");
            }

            try
            {
               
                //dá ordem de remoção do Agente
                db.Agentes.Remove(agentes);
                //consolida a remoção
                db.SaveChanges();

            }
            catch (Exception)
            {
                //devem aqui ser escritas todas as instruções que são consideradas necessarias

                //informar que houve um erro
                ModelState.AddModelError("", "Não é possivel remover o Agente "+agentes.Nome+". Provavelmente, tem multas associadas a ele...");

                //redirecionar para a pagina onde o erro foi gerado
                return View(agentes);
            }
            

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
