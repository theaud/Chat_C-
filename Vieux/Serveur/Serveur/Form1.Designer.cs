namespace Serveur
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.Log = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Utilisateurs = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Discussion = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Log
            // 
            this.Log.FormattingEnabled = true;
            this.Log.Location = new System.Drawing.Point(12, 29);
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(275, 121);
            this.Log.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Log Serveur";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(308, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Utilisateurs";
            // 
            // Utilisateurs
            // 
            this.Utilisateurs.FormattingEnabled = true;
            this.Utilisateurs.Location = new System.Drawing.Point(311, 29);
            this.Utilisateurs.Name = "Utilisateurs";
            this.Utilisateurs.Size = new System.Drawing.Size(97, 121);
            this.Utilisateurs.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Discussion";
            // 
            // Discussion
            // 
            this.Discussion.FormattingEnabled = true;
            this.Discussion.Location = new System.Drawing.Point(12, 188);
            this.Discussion.Name = "Discussion";
            this.Discussion.Size = new System.Drawing.Size(396, 186);
            this.Discussion.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 384);
            this.Controls.Add(this.Discussion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Utilisateurs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Log);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox Log;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox Utilisateurs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox Discussion;
    }
}

