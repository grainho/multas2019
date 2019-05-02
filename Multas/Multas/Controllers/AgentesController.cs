using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agentes){
            if (ModelState.IsValid)
            {
                db.Agentes.Add(agentes);
                db.SaveChanges();
                return RedirectToAction("Index");
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
